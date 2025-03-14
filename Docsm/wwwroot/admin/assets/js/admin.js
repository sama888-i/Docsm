// Həkimləri statusa görə gətirən funksiya
async function fetchDoctorsByStatus(status) {
    try {
        let url = "/api/Admin/GetDoctors";
        if (status) {
            url += `?status=${status}`;
        }

        const response = await fetch(url);
        if (!response.ok) {
            throw new Error("Həkim məlumatlarını gətirile bilmədi");
        }
        return await response.json(); // JSON-a çevir və qaytar
    } catch (error) {
        console.error("Xəta baş verdi:", error);
        return [];
    }
}

async function updateDoctorsTable() {
    const selectedStatus = document.getElementById("doctorStatus").value;
    const tableBody = document.getElementById("doctors-table");

    tableBody.innerHTML = `<tr><td colspan="5">Yüklənir...</td></tr>`;

    const doctors = await fetchDoctorsByStatus(selectedStatus);

    if (doctors.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="5">Məlumat yoxdur</td></tr>`;
        return;
    }

    tableBody.innerHTML = doctors.map(doctor => `
        <tr>
            <td><img src="/images/doctors/${doctor.profileImageUrl}" width="50" height="50" style="border-radius: 50%"></td>
            <td>Dr.${doctor.name} ${doctor.surname}</td>
            <td>${doctor.specialtyName}</td>
            <td>${doctor.clinicName} (${doctor.clinicAddress})</td>
            <td>${doctor.averageRating.toFixed(1)} ⭐</td>
            <td>
                <button class="btn btn-success btn-sm" onclick="approveDoctor(${doctor.id})">Approve</button>
                <button class="btn btn-danger btn-sm" onclick="rejectDoctor(${doctor.id})">Reject</button>
            </td>
        </tr>
    `).join('');
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("doctorStatus").addEventListener("change", updateDoctorsTable);
});
function approveDoctor(doctorId) {
    fetch(`/api/Admin/ApproveDoctor?doctorId=${doctorId}`, {
        method: "POST"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to approve doctor");
            }
            return response.text(); // JSON olmayan cavab üçün .text() oxuyuruq
        })
        .then(text => {
            try {
                const data = JSON.parse(text); // Əgər JSON-dursa parse et
                alert(data.message);
            } catch {
                alert("Doctor approved successfully!");
            }
            updateDoctorsTable();
        })
        .catch(error => console.error("Error approving doctor:", error));
}

function rejectDoctor(doctorId) {
    fetch(`/api/Admin/RejectDoctor?doctorId=${doctorId}`, {
        method: "POST"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to reject doctor");
            }
            return response.text(); // JSON olmayan cavab üçün .text() oxuyuruq
        })
        .then(text => {
            try {
                const data = JSON.parse(text); // Əgər JSON-dursa parse et
                alert(data.message);
            } catch {
                alert("Doctor rejected successfully!");
            }
            updateDoctorsTable();
        })
        .catch(error => console.error("Error rejecting doctor:", error));
}
async function fetchPatients() {
    try {
        const response = await fetch("/api/Admin/AllPatients");
        if (!response.ok) {
            throw new Error("Pasiyentlər gətirile bilmədi.");
        }
        return await response.json();
    } catch (error) {
        console.error("Xəta:", error);
        return [];
    }
}

// 🔹 Pasiyentlər cədvəlini yeniləyən funksiya
async function updatePatientsTable() {
    const tableBody = document.getElementById("patients-table");
    tableBody.innerHTML = `<tr><td colspan="4">Yüklənir...</td></tr>`;

    const patients = await fetchPatients();

    if (patients.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="4">Məlumat yoxdur</td></tr>`;
        return;
    }

    tableBody.innerHTML = patients.map(patient => `
        <tr>
            <td><img src="/images/patients/${patient.patientImage}" width="50" height="50" style="border-radius: 50%"></td>
            <td>${patient.patientName}</td>
            <td>${patient.email}</td>
            <td>${patient.phoneNumber}</td>
        </tr>
    `).join('');
}

// 🔹 Səhifə yüklənən kimi pasiyentləri yenilə
document.addEventListener("DOMContentLoaded", () => {
    updatePatientsTable();
});
async function fetchReviews() {
    try {
        const response = await fetch("/api/Admin/GetReviews");
        if (!response.ok) {
            throw new Error("Rəylər gətirile bilmədi.");
        }
        return await response.json();
    } catch (error) {
        console.error("Xəta:", error);
        return [];
    }
}

// 🔹 Rəyləri cədvələ yerləşdiririk
async function updateReviewsTable() {
    const tableBody = document.getElementById("reviews-table");
    tableBody.innerHTML = `<tr><td colspan="5">Yüklənir...</td></tr>`;

    const reviews = await fetchReviews();

    if (reviews.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="5">Məlumat yoxdur</td></tr>`;
        return;
    }

    tableBody.innerHTML = reviews.map(review => `
        <tr id="review-row-${review.Id}">
            <td><img src="/images/doctors/${review.doctorImage}" width="50" height="50" style="border-radius: 50%"> ${review.doctorFullname}</td>
            <td><img src="/images/patients/${review.patientImage}" width="50" height="50" style="border-radius: 50%"> ${review.patientFullname}</td>
            <td>${review.comment}</td>
            <td>${review.rating} ⭐</td>
            <td>
                <button data-id="${review.id}" class="btn btn-sm bg-danger-light delete-btn" href="#delete_modal">
                                        <i class="fe fe-trash"></i> Delete</button>
            </td>
        </tr>
    `).join('');
}

// 🔹 Səhifə yüklənən kimi rəyləri yenilə
document.addEventListener("DOMContentLoaded", () => {
    updateReviewsTable();
});

$(document).on('click', '.delete-btn', function () {
    var reviewId = $(this).data('id');
    if (confirm('Bu commenti silmək istədiyinizə əminsiniz?')) {
        fetch(`/api/Review/${reviewId}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Comment silinərkən səhv baş verdi!');
                }
                $(`#review-row-${reviewId}`).remove();
                updateReviewsTable();
                alert('Comment uğurla silindi!');
            })
            .catch(err => {
                console.error('Xəta:', err);
                alert('Xəta: Comment silinərkən səhv baş verdi!');
            });
    }
});
async function fetchAllTransactions() {
    const response = await fetch('/api/Admin/Transaction');
    if (!response.ok) {
        throw new Error('Tranzaksiyalar alınarkən xəta baş verdi');
    }
    return await response.json();
}

// Tranzaksiya cədvəlini yeniləyir
async function updateTransactionTable() {
    const tableBody = document.getElementById("transaction-table-body");
    tableBody.innerHTML = `<tr><td colspan="6">Yüklənir...</td></tr>`;

    try {
        const transactions = await fetchAllTransactions();
        tableBody.innerHTML = transactions.map(transaction => `
            <tr>
                <td>${transaction.transactionId}</td>
                <td>
                    <img src="/images/patients/${transaction.patientImage || 'default-patient.jpg'}" width="50" height="50" style="border-radius: 50%">
                    ${transaction.patientName}
                </td>
                <td>
                    <img src="/images/doctors/${transaction.doctorImage || 'default-doctor.jpg'}" width="50" height="50" style="border-radius: 50%">
                    ${transaction.doctorName}
                </td>
                <td>${transaction.amount} ${transaction.currency}</td>
                <td>${transaction.paymentStatus}</td>
                <td>${transaction.appointmentDate}</td>
            </tr>
        `).join('');
    } catch (error) {
        console.error('Xəta:', error);
        tableBody.innerHTML = `<tr><td colspan="6">Xəta: ${error.message}</td></tr>`;
    }
}

// Səhifə yüklənərkən tranzaksiya cədvəlini yeniləyir
document.addEventListener("DOMContentLoaded", () => {
    updateTransactionTable();
});
async function fetchAllAppointments() {
    const response = await fetch('/api/Admin/Appointments');
    if (!response.ok) {
        throw new Error('Görüşlər alınarkən xəta baş verdi');
    }
    return await response.json();
    
}

// Görüş cədvəlini yeniləyir
async function updateAppointmentTable() {
    const tableBody = document.getElementById("appointment-table-body");
    tableBody.innerHTML = `<tr><td colspan="5">Yüklənir...</td></tr>`;

    try {
        const appointments = await fetchAllAppointments();
        tableBody.innerHTML = appointments.map(appointment => `
            <tr>
                <td><img src="/images/doctors/${appointment.doctorImage || 'default-doctor.jpg'}" width="50" height="50" style="border-radius: 50%"> ${appointment.doctorName}</td>
                <td><img src="/images/patients/${appointment.patientImage || 'default-patient.jpg'}" width="50" height="50" style="border-radius: 50%"> ${appointment.patientName}</td>
                <td>${appointment.appointmentDate}</td>
                <td>${appointment.amount}</td>
                <td>${appointment.status}</td>
            </tr>
        `).join('');
        console.log(appointments);
    } catch (error) {
        console.error('Xəta:', error);
        tableBody.innerHTML = `<tr><td colspan="5">Xəta: ${error.message}</td></tr>`;
    }
}

// Səhifə yüklənərkən görüş cədvəlini yeniləyir
document.addEventListener("DOMContentLoaded", () => {
    updateAppointmentTable();
});

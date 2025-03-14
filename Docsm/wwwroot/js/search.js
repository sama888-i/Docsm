// Fetch specialties from the backend
async function fetchSpecialties() {
    try {
        const response = await fetch('/api/Specialty'); 
        const specialties = await response.json();
        const specialtyList = document.getElementById('specialty-list');

        specialties.forEach(specialty => {
            const label = document.createElement('label');
            label.classList.add('custom_check');
            label.innerHTML = `
                <input type="checkbox" name="select_specialist" value="${specialty.id}">
                <span class="checkmark"></span> ${specialty.name}
            `;
            specialtyList.appendChild(label);
        });
    } catch (error) {
        console.error('Error fetching specialties:', error);
    }
}

// Call this function to load specialties when the page loads
document.addEventListener('DOMContentLoaded', fetchSpecialties);

// Function to trigger search for doctors based on selected filters
async function searchDoctors() {
    const selectedSpecialties = Array.from(document.querySelectorAll('input[name="select_specialist"]:checked'))
        .map(input => input.value);
    const selectedGender = document.querySelector('input[name="gender_type"]:checked')?.value; // Seçilen genderi düzgün əldə et

    const query = new URLSearchParams();

    if (selectedSpecialties.length) {
        query.append('specialtyId', selectedSpecialties.join(',')); // Specialty seçimi
    }
    if (selectedGender) {
        query.append('gender', selectedGender); // Gender seçimi (Male ya da Female)
    }

    try {
        const response = await fetch(`/api/Search?${query.toString()}`);
        if (response.ok) {
            const doctors = await response.json();
            displayDoctors(doctors);
        } else {
            console.error('Error searching doctors:', response.status);
        }
    } catch (error) {
        console.error('Error searching doctors:', error);
    }
}


// Function to display doctor results
function displayDoctors(doctors) {
    const resultContainer = document.querySelector('.col-md-12.col-lg-8.col-xl-9');
    resultContainer.innerHTML = ''; // Clear existing results

    doctors.forEach(doctor => {
        const doctorCard = document.createElement('div');
        doctorCard.classList.add('card');
        const ratingStars = getRatingStars(doctor.rating); 
        doctorCard.innerHTML = `
            <div class="card-body">
                <div class="doctor-widget">
                    <div class="doc-info-left">
                        <div class="doctor-img">
                            <a href="doctor-profile.html">
                               <img src="/images/doctors/${doctor.doctorImage || 'default.jpg'}" class="img-fluid" alt="Doctor Image">
                            </a>
                        </div>
                        <div class="doc-info-cont">
                            <h4 class="doc-name"><a href="doctor-profile.html">${doctor.doctorName}</a></h4>
                            <p class="doc-speciality">${doctor.specialty}</p>
                           <div class="rating">
                                ${ratingStars}
                                <span class="d-inline-block average-rating">(${doctor.rating})</span>
                            </div>
                            <p class="doc-location"><i class="fas fa-map-marker-alt"></i> ${doctor.clinicAddress}</p>
                        </div>
                    </div>
                    <div class="doc-info-right">
                        <div class="col-6">
                                <a href="doctor-profile.html?id=${doctor.id}" class="btn view-btn">View Profile</a>
                            </div>
                            <div class="col-6">
                                <a href="javascript:void(0)" class="btn book-btn" onclick="openAppointmentModal(${doctor.id})">Book Now</a>
                            </div>
                    </div>
                </div>
            </div>
        `;
        resultContainer.appendChild(doctorCard);
    });
}
function getRatingStars(rating) {
    let stars = '';
    const fullStars = Math.floor(rating); // Tam ulduz sayını hesablayır
    const halfStars = (rating % 1) >= 0.5 ? 1 : 0; // Yarım ulduz var mı? (yarım ulduz için .5 və daha yüksək qiymət)

    // Tam ulduzları əlavə et
    for (let i = 0; i < fullStars; i++) {
        stars += '<i class="fas fa-star filled"></i>';
    }

    // Yarım ulduzu əlavə et (əgər varsa)
    if (halfStars) {
        stars += '<i class="fas fa-star-half-alt filled"></i>';
    }

    // Boş ulduzları əlavə et
    for (let i = fullStars + halfStars; i < 5; i++) {
        stars += '<i class="fas fa-star"></i>';
    }

    return stars;
}
document.querySelectorAll('input[name="gender_type"]').forEach((checkbox) => {
    checkbox.addEventListener('click', function () {
        // Əgər checkbox-ın üzərinə kliklənibsə, statusu dəyişirik
        if (this.checked) {
            // Seçim etmədikdə qalan checkbox-ların heç biri seçilmir
            document.querySelectorAll('input[name="gender_type"]').forEach((otherCheckbox) => {
                if (otherCheckbox !== this) {
                    otherCheckbox.checked = false; // Qalanını geri alırıq
                }
            });
        } else {
            // Əgər artıq seçilmişsə, seçimi silirik
            this.checked = false;
        }
    });
});
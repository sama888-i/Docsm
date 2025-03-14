$(document).ready(function () {
    // Yeni ixtisas əlavə etmək
    $('#Add_Specialities_details form').submit(function (e) {
        e.preventDefault();

        var specialityName = $("input[name='speciality_name']").val();
        var specialityImage = $("input[name='speciality_image']").prop('files')[0];

        var formData = new FormData();
        formData.append("name", specialityName);
        formData.append("image", specialityImage);

        fetch('/api/Specialty', {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('İxtisas əlavə edilərkən səhv baş verdi!');
            }
            return response.json();  // JSON olaraq cavabı alırıq
        })
        .then(data => {
            var newSpeciality = `<tr id="speciality-row-${data.id}">
                                    <td>#SP${data.id}</td>
                                    <td>
                                        <h2 class="table-avatar">
                                            <a href="profile.html" class="avatar avatar-sm mr-2">
                                                <img class="avatar-img" src="${data.image}" alt="Speciality">
                                            </a>
                                            <a href="profile.html">${data.name}</a>
                                        </h2>
                                    </td>
                                    <td class="text-right">
                                        <div class="actions">
                                            <a class="btn btn-sm bg-success-light edit-btn" data-id="${data.id}" href="#edit_specialities_details">
                                                <i class="fe fe-pencil"></i> Edit
                                            </a>
                                            <a data-id="${data.id}" class="btn btn-sm bg-danger-light delete-btn" href="#delete_modal">
                                                <i class="fe fe-trash"></i> Delete
                                            </a>
                                        </div>
                                    </td>
                                </tr>`;
            $('.datatable tbody').append(newSpeciality);
            $('#Add_Specialities_details').modal('hide');
            alert('İxtisas uğurla əlavə olundu!');
            loadSpecialties();

            // Formu sıfırlayırıq
            $("input[name='speciality_name']").val('');
            $("input[name='speciality_image']").val('');
            $("#speciality_image_preview").attr("src", '');  
        })
        .catch(err => {
            console.error('Xəta:', err);
            alert('Xəta: İxtisas əlavə edilərkən səhv baş verdi!');
        });
    });
    $('#Add_Specialities_details').on('show.bs.modal', function () {
        // Formu sıfırlama
        $("input[name='speciality_name']").val('');  // name sahəsini sıfırlamaq
        $("input[name='speciality_image']").val(''); // image sahəsini sıfırlamaq
        $("#speciality_image_preview").attr("src", '');  // şəkil önizləməsini sıfırlamaq
    });
    // Bütün ixtisasları yüklə
    function loadSpecialties() {
        fetch('/api/Specialty')
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => { throw new Error(text || 'İxtisaslar yüklənərkən səhv baş verdi!'); });
                }
                return response.json();
            })
            .then(data => {
                console.log("Gələn ixtisaslar:", data); // Debug üçün

                if (!Array.isArray(data)) {
                    throw new Error("Gələn data array deyil!");
                }

                let tableBody = $("#specialtyTable tbody");
                tableBody.empty(); // Cədvəli təmizlə

                data.forEach(specialty => {
                    var specialityRow = `
                   <tr id="speciality-row-${specialty.id}">
                            <td>#SP${specialty.id}</td>
                            <td>
                                <h2 class="table-avatar">
                                    <a href="profile.html" class="avatar avatar-sm mr-2">
                                        <img class="avatar-img" src="/images/specialties/${specialty.imageUrl}" alt="Speciality">
                                    </a>
                                    <a href="profile.html">${specialty.name}</a> <!-- Bu hissə adı ehtiva edir -->
                                </h2>
                            </td>
                            <td class="text-right">
                                <div class="actions">
                                    <a class="btn btn-sm bg-success-light edit-btn" data-id="${specialty.id}" href="#edit_specialities_details">
                                        <i class="fe fe-pencil"></i> Edit
                                    </a>
                                    <a data-id="${specialty.id}" class="btn btn-sm bg-danger-light delete-btn" href="#delete_modal">
                                        <i class="fe fe-trash"></i> Delete
                                    </a>
                                </div>
                            </td>
                        </tr>`;
                    tableBody.append(specialityRow);
                });
            })
            .catch(err => {
                console.error('Xəta:', err);
                alert('Xəta: İxtisaslar yüklənərkən səhv baş verdi!');
            });
    }

    $(document).ready(function () {
        loadSpecialties();
    });
    // Modal açıldıqda input-ları təmizlə
    $('#edit_specialities_details').on('hidden.bs.modal', function () {
        $(this).find("input").val('');
        $("#speciality_image_preview").attr("src", '');
    });
    // Modal açılmadan əvvəl ixtisasın məlumatlarını əldə et
    $(document).on('click', '.edit-btn', function () {
        // Modalı boş açmaq
        $('#edit_specialities_details').modal('show');
    });

    // Modalda məlumatları göndərmək üçün formu işlətmək
    $('#editSpecialtyForm').on('submit', function (e) {
        e.preventDefault();

        var specialityId = $("input[name='speciality_id']").val();  // İxtisasın ID-si
        var specialityName = $("input[name='speciality_name']").val();  // İxtisasın adı
        var specialityImage = $("input[name='speciality_image']").prop('files')[0];  // Şəkil

        var formData = new FormData();
        formData.append("id", specialityId);
        formData.append("name", specialityName);
        if (specialityImage) {
            formData.append("image", specialityImage);  // Şəkil varsa, onu da əlavə edirik
        }

        // FormData içində nə olduğunu yoxla
        for (let [key, value] of formData.entries()) {
            console.log(key, value);  // FormData içindəki məlumatları yoxlayın
        }

        // PUT sorğusunu göndəririk
        fetch(`/api/Specialty/${specialityId}`, {
            method: 'PUT',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Xəta baş verdi');
                }
                return response.json();
            })
            .then(updatedData => {
                console.log('Yenilənmiş Məlumat:', updatedData);
                $('#edit_specialities_details').modal('hide');

                // Yenilənmiş məlumatı səhifəyə tətbiq et
                $(`#specialty_row_${updatedData.id} .specialty_name`).text(updatedData.name);
                if (updatedData.imageUrl) {
                    $(`#specialty_row_${updatedData.id} .specialty_image`).attr("src", `/images/specialties/${updatedData.imageUrl}`);
                } else {
                    $(`#specialty_row_${updatedData.id} .specialty_image`).attr("src", '/images/placeholder.jpg');
                }
            })
            .catch(err => {
                console.error('Xəta:', err);
                alert('Xəta: İxtisas yenilənə bilmədi.');
            });
    });





    // İxtisası silmək
    $(document).on('click', '.delete-btn', function () {
        var specialityId = $(this).data('id');
        if (confirm('Bu ixtisası silmək istədiyinizə əminsiniz?')) {
            fetch(`/api/Specialty/${specialityId}`, {
                method: 'DELETE'
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('İxtisas silinərkən səhv baş verdi!');
                }
                $(`#speciality-row-${specialityId}`).remove();
                alert('İxtisas uğurla silindi!');
            })
            .catch(err => {
                console.error('Xəta:', err);
                alert('Xəta: İxtisas silinərkən səhv baş verdi!');
            });
        }
    });
});


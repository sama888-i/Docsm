document.addEventListener("DOMContentLoaded", function () {
    fetchSpecialties();  // Məlumatları almağa başla
});

function fetchSpecialties() {
    fetch('/api/Specialty') 
        .then(response => {
            if (!response.ok) {
                throw new Error('Məlumatlar alınmadı!');
            }
            return response.json();
        })
        .then(data => {
            const sliderContainer = document.querySelector(".specialities-slider");  

            // Sliderı əvvəlcə "unslick" edirik ki, əvvəlki sürüşmələr silinsin
            $(sliderContainer).slick('unslick');

            // Slider üçün yeni item-ləri yaratmaq
            data.forEach(specialty => {
                const item = document.createElement("div");  // Yeni bir div yaradırıq
                item.classList.add("speicality-item", "text-center");  // Lazım olan sinifləri əlavə edirik

                item.innerHTML = `
                    <div class="speicality-img">
                        <img src="/images/specialties/${specialty.imageUrl}" class="img-fluid" alt="Speciality">
                        <span><i class="fa fa-circle" aria-hidden="true"></i></span>
                    </div>
                    <p>${specialty.name}</p>
                `;  // HTML strukturu daxil edirik

                sliderContainer.appendChild(item);  // Yaradılan itemi slider-a əlavə edirik
            });

            // Slick slider-ı yenidən başlatmaq
            initSlickSlider();
        })
        .catch(error => {
            console.error("Xəta baş verdi:", error);
        });
}

// Slick slider-ı başlatmaq
function initSlickSlider() {
    $(".specialities-slider").slick({
        dots: true,  // Hər bir item üçün nöqtələr göstərilsin
        infinite: true,  // Sonsuz dövr
        speed: 300,  // Keçid sürəti
        slidesToShow: 4,  // Bir anda neçə item göstərilsin
        slidesToScroll: 1,  // Hər keçiddə bir item sürüşsün
        arrows: true,  // Oxlar aktiv olsun
        swipe: true,  // Toxunma ilə sürüşdürmə aktiv olsun
        touchMove: true,  // Toxunma ilə hərəkət aktiv olsun
        draggable: true,  // Mouse ilə sürüşdürmə aktiv olsun
        responsive: [
            {
                breakpoint: 768,  // Kiçik ekranlar üçün
                settings: {
                    slidesToShow: 1,  // 1 item göstərilsin
                }
            }
        ]
    });
}
function fetchTopDoctors() {
    fetch('/api/Search/top-doctors')
        .then(response => {
            if (!response.ok) {
                throw new Error('Məlumatlar alınmadı!');
            }
            return response.json();
        })
        .then(doctors => {
            const sliderContainer = document.querySelector(".doctor-slider");
            $(sliderContainer).slick('unslick'); // Mövcud sliderı silirik

            sliderContainer.innerHTML = ""; // Mövcud həkimləri təmizləyirik

            // Həkimlərin widget-larını yaratmaq
            doctors.forEach(doctor => {
                const item = document.createElement("div");
                item.classList.add("profile-widget");

                item.innerHTML = `
                    <div class="doc-img">
                        <a href="doctor-profile.html?id=${doctor.id}">
                            <img class="img-fluid" alt="User Image" src="/images/doctors/${doctor.profileImageUrl}">
                        </a>
                        <a href="javascript:void(0)" class="fav-btn">
                            <i class="far fa-bookmark"></i>
                        </a>
                    </div>
                    <div class="pro-content">
                        <h3 class="title">
                            <a href="doctor-profile.html?id=${doctor.id}">Dr. ${doctor.name} ${doctor.surname}</a>
                            <i class="fas fa-check-circle verified"></i>
                        </h3>
                        <p class="speciality">${doctor.specialty}</p>
                        <div class="rating">
                            ${generateRatingStars(doctor.rating)}
                            <span class="d-inline-block average-rating">(${doctor.rating.toFixed(1)})</span>
                        </div>
                        <ul class="available-info">
                            <li><i class="fas fa-map-marker-alt"></i> ${doctor.clinicAddress}</li>
                            <li><i class="far fa-money-bill-alt"></i> ${doctor.perAppointPrice} azn</li>
                        </ul>
                        <div class="row row-sm">
                            <div class="col-6">
                                <a href="doctor-profile.html?id=${doctor.id}" class="btn view-btn">View Profile</a>
                            </div>
                            <div class="col-6">
                                <a href="javascript:void(0)" class="btn book-btn" onclick="openAppointmentModal(${doctor.id})">Book Now</a>
                            </div>
                        </div>
                    </div>
                `;

                sliderContainer.appendChild(item);
            });

            initDoctorSlickSlider(); // Sliderı yenidən başlatmaq
        })
        .catch(error => {
            console.error("Xəta baş verdi:", error);
        });
}

// **Slick slider-ı başlatmaq**
function initDoctorSlickSlider() {
    $(".doctor-slider").slick({
        dots: true,
        infinite: true,
        speed: 300,
        slidesToShow: 3,
        slidesToScroll: 1,
        arrows: true,
        swipe: true,
        touchMove: true,
        draggable: true,
        adaptiveHeight: false,       
        variableWidth: false, 
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });
}

// **Ulduz rating funksiyası**
function generateRatingStars(rating) {
    let stars = "";
    for (let i = 1; i <= 5; i++) {
        stars += `<i class="fas fa-star ${i <= rating ? 'filled' : ''}"></i>`;
    }
    return stars;
}

// **Səhifə yüklənəndə həkimləri göstər**
document.addEventListener("DOMContentLoaded", fetchTopDoctors);

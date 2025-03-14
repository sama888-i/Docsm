function loadHeaderScripts() {
    console.log(" loadHeaderScripts çağırıldı!");

    setTimeout(() => {
        const token = localStorage.getItem("token");

        // Elementləri tapırıq
        const loginLink = document.getElementById("loginLink");
        const logoutLink = document.getElementById("logoutLink");
        const doctorMenu = document.getElementById("doctorMenu");
        const patientMenu = document.getElementById("patientMenu");
        const adminMenu = document.getElementById("adminMenu");

        console.log(" Login Link:", loginLink);
        console.log(" Logout Link:", logoutLink);
        console.log(" Doctor Menu:", doctorMenu);
        console.log(" Patient Menu:", patientMenu);
        console.log(" Admin Menu:", adminMenu);

        if (!loginLink || !logoutLink || !doctorMenu || !patientMenu || !adminMenu) {
            console.warn(" Bəzi elementlər tapılmadı, script dayandırıldı.");
            return;
        }

        if (token) {
            const decodedToken = parseJwt(token);
            const userRole = decodedToken["role"];
            console.log("User Role:", userRole);

            loginLink.style.display = "none";
            logoutLink.style.display = "block";

            if (userRole === "Doctor") {
                doctorMenu.style.display = "block";
            } else if (userRole === "Patient") {
                patientMenu.style.display = "block";
            } else if (userRole === "Admin") {
                adminMenu.style.display = "block";
            }
        } else {
            loginLink.style.display = "block";
            logoutLink.style.display = "none";
        }
    }, 500); 
}


// Token-i decode edən funksiya
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error("Invalid token:", error);
        return {};
    }
}

// Logout funksiyası
function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html"; // Logout edəndən sonra login səhifəsinə yönləndiririk
}

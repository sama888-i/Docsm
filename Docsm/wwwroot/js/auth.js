const API_URL = "https://localhost:7274/api/Auth";

document.addEventListener("DOMContentLoaded", function () {
    setupForm("loginForm", handleLogin);
    setupForm("registerForm", handleRegister);
    setupForm("registerFormForDoctor", handleDoctorRegister);
    setupForm("forgotPassword", handleForgotPassword);
    setupForm("resetPassword", handleResetPassword);
    setupForm("emailConfirm", handleEmailConfirm);
   
});

// 🔹 Formun event listenerini avtomatik təyin edir
function setupForm(formId, handlerFunction) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener("submit", function (event) {
            event.preventDefault();
            handlerFunction(event);
        });
    }
}

// 🔹 Login prosesini idarə edən funksiya
async function handleLogin() {
    console.log("Login form submitted!");

    const UserNameOrEmail = document.getElementById("UserNameOrEmail").value.trim();
    const Password = document.getElementById("Password").value.trim();
    const RememberMe = document.getElementById("RememberMe").checked;

    document.getElementById("error-container").innerText = "";

    try {
        let response = await fetch(`${API_URL}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ UserNameOrEmail, Password, RememberMe })
        });

        let data = await response.json();

        if (!response.ok) {
            document.getElementById("error-container").innerText = data.message || "Login failed.";
            return;
        }

        localStorage.setItem("token", data.token);  
        loadHeaderScripts(); 
        window.location.href = "index.html";
    } catch (error) {
        console.error("Error:", error);
        document.getElementById("error-container").innerText = "An unexpected error occurred.";
    }
}

// 🔹 Register prosesini idarə edən funksiya
async function handleRegister() {
    console.log("Register form submitted!");

    document.getElementById("error-container").innerText = "";
    document.getElementById("success-container").innerText = "";

    let dateOfBirthInput = document.getElementById("DateOfBirth").value;
    let formattedDateOfBirth = dateOfBirthInput ? new Date(dateOfBirthInput).toISOString().split('T')[0] : null;

    if (!dateOfBirthInput) {
        document.getElementById("error-container").innerText = "Date of Birth is required.";
        return;
    }

    let formData = {
        UserName: document.getElementById("UserName").value.trim(),
        Email: document.getElementById("Email").value.trim(),
        Name: document.getElementById("Name").value.trim(),
        Surname: document.getElementById("Surname").value.trim(),
        Password: document.getElementById("Password").value.trim(),
        DateOfBirth: formattedDateOfBirth,
        Gender: document.getElementById("Gender").value
    };

    try {
        let response = await fetch(`${API_URL}/RegisterForPatient`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(formData)
        });

        let result = await response.json();

        if (!response.ok) {
            if (response.status === 409) {
                if (result.message.includes("Email")) {
                    document.getElementById("EmailError").innerText = "Email already exists.";
                }
                if (result.message.includes("Username")) {
                    document.getElementById("UserNameError").innerText = "Username already exists.";
                }
            } else if (result.errors) {
                Object.keys(result.errors).forEach(field => {
                    document.getElementById(`${field}Error`).innerText = result.errors[field].join(", ");
                });
            }
            throw new Error(result.message || "Validation failed.");
        }

        document.getElementById('success-container').textContent = result.Message || "Registration successful!";
        setTimeout(() => {
            window.location.href = "email-confirm.html";
        }, 2000);
    } catch (error) {
        document.getElementById('error-container').textContent = error.message || "An unexpected error occurred.";
    }
}
async function handleDoctorRegister() {
    console.log("Doctor Register form submitted!");

    document.getElementById("error-container").innerText = "";
    document.getElementById("success-container").innerText = "";

    let dateOfBirthInput = document.getElementById("DateOfBirth").value;
    let formattedDateOfBirth = dateOfBirthInput ? new Date(dateOfBirthInput).toISOString().split('T')[0] : null;

    if (!dateOfBirthInput) {
        document.getElementById("error-container").innerText = "Date of Birth is required.";
        return;
    }

    let formData = {
        UserName: document.getElementById("UserName").value.trim(),
        Email: document.getElementById("Email").value.trim(),
        Name: document.getElementById("Name").value.trim(),
        Surname: document.getElementById("Surname").value.trim(),
        Password: document.getElementById("Password").value.trim(),
        DateOfBirth: formattedDateOfBirth,
        Gender: document.getElementById("Gender").value
    };

    try {
        let response = await fetch(`${API_URL}/RegisterForDoctor`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(formData)
        });

        let result = await response.json();

        if (!response.ok) {
            if (response.status === 409) {
                if (result.message.includes("Email")) {
                    document.getElementById("EmailError").innerText = "Email already exists.";
                }
                if (result.message.includes("Username")) {
                    document.getElementById("UserNameError").innerText = "Username already exists.";
                }
            } else if (result.errors) {
                Object.keys(result.errors).forEach(field => {
                    document.getElementById(`${field}Error`).innerText = result.errors[field].join(", ");
                });
            }
            throw new Error(result.message || "Validation failed.");
        }

        document.getElementById('success-container').textContent = result.Message || "Registration successful!";
        setTimeout(() => {
            window.location.href = "email-confirm.html";
        }, 2000);
    } catch (error) {
        document.getElementById('error-container').textContent = error.message || "An unexpected error occurred.";
    }
}
async function handleForgotPassword(event) {
    // Form submitini bloklayırıq
    event.preventDefault();

    const email = document.getElementById("ForgotPasswordEmail").value.trim();
    document.getElementById("error-container").innerText = "";
    document.getElementById("success-container").innerText = "";

    if (!email) {
        document.getElementById("error-container").innerText = "Email is required.";
        return;
    }

    try {
        let response = await fetch(`${API_URL}/ForgotPassword`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Email: email })
        });

        let result = await response.text(); // API string qaytarır

        if (!response.ok) {
            throw new Error(result || "Failed to send reset email.");
        }

        document.getElementById("success-container").innerText = result;
        setTimeout(() => {
            window.location.href = `reset-password.html?email=${encodeURIComponent(email)}`;
        }, 2000);
    } catch (error) {
        document.getElementById("error-container").innerText = error.message;
    }
   
}
async function handleResetPassword(event) {
    event.preventDefault(); 

    const email = document.getElementById("resetEmail").value.trim();
    const code = document.getElementById("resetCode").value.trim();
    const newPassword = document.getElementById("newPassword").value.trim();

    document.getElementById("error-container").innerText = "";
    document.getElementById("success-container").innerText = "";

    if (!email || !code || !newPassword) {
        document.getElementById("error-container").innerText = "All fields are required.";
        return;
    }

    try {
        let response = await fetch(`${API_URL}/ResetPassword`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Email: email,
                Code: parseInt(code), // Backend int gözləyir
                NewPassword: newPassword
            })
        });

        let result = await response.text(); // API string response qaytarır

        if (!response.ok) {
            throw new Error(result || "Failed to reset password.");
        }

        document.getElementById("success-container").innerText = result;

        setTimeout(() => {
            window.location.href = "login.html";
        }, 2000);
    } catch (error) {
        document.getElementById("error-container").innerText = error.message;
    }
}
async function handleEmailConfirm(event) {
    event.preventDefault(); // Formun avtomatik submit edilməsinin qarşısını alırıq

    const email = document.getElementById("confirmEmail").value.trim();
    const code = document.getElementById("confirmCode").value.trim();

    document.getElementById("error-container").innerText = "";
    document.getElementById("success-container").innerText = "";

    if (!email || !code) {
        document.getElementById("error-container").innerText = "All fields are required.";
        return;
    }

    try {
        let response = await fetch(`${API_URL}/EmailConfirm?email=${encodeURIComponent(email)}&code=${code}`, {
            method: "GET"
        });

        let result = await response.text(); // API string cavab qaytarır

        if (!response.ok) {
            throw new Error(result || "Email confirmation failed.");
        }

        document.getElementById("success-container").innerText = result;

        setTimeout(() => {
            window.location.href = "login.html";
        }, 2000);
    } catch (error) {
        document.getElementById("error-container").innerText = error.message;
    }
}

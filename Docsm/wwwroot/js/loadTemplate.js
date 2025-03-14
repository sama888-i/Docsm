async function loadComponent(id, file, callback) {
    let element = document.getElementById(id);
    if (element) {
        let response = await fetch(file);
        let html = await response.text();
        element.innerHTML = html;

        console.log(` ${id} yükləndi!`);

        if (callback) callback();
    }
}

window.onload = function () {
    const token = localStorage.getItem("token");
    if (token) {
        loadHeaderScripts(); 
    }

    loadComponent("header", "/partials/header.html", function () {
        console.log(" Header yükləndi");
      
    });

    loadComponent("footer", "/partials/footer.html");
};

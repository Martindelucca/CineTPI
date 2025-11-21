// Este script se ejecuta INMEDIATAMENTE (desde <head>)

const token = localStorage.getItem('token');

// Si no hay token, redirige a login SOLO en páginas protegidas
if (!token) {
    // Lista de páginas que requieren login
    const paginasProtegidas = [
        "admin.html",
        "dashboard.html",
        "soporte-peliculas.html",
        "funciones.html",
        "butacas.html"
    ];

    const paginaActual = window.location.pathname.split("/").pop();

    if (paginasProtegidas.includes(paginaActual)) {
        window.location.href = "login.html";
    }
}

// Función genérica para cerrar sesión
function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("usuarioNombre");
    localStorage.removeItem("rol");
    window.location.href = "login.html";
}

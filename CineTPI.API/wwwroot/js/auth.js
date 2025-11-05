// Este script se ejecuta INMEDIATAMENTE (desde el <head>)

// 1. Buscamos el token en el almacén local del navegador
const token = localStorage.getItem('token');

// 2. Verificamos si existe
if (!token) {
    // 3. Si NO hay token, el usuario no ha iniciado sesión.
    // Lo "pateamos" de vuelta a la página de login.
    console.warn('Acceso denegado. No se encontró token.');
    window.location.href = 'login.html';
}

// Si el token SÍ existe, el script termina y la página (index.html)
// se carga normalmente.
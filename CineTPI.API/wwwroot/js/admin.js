document.addEventListener("DOMContentLoaded", () => {
  const nombre = localStorage.getItem("usuarioNombre");
  const nombreSpan = document.getElementById("admin-nombre");
  if (nombre) nombreSpan.textContent = nombre;

  const btnLogout = document.getElementById("btn-logout");
  btnLogout.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "login.html";
  });
});

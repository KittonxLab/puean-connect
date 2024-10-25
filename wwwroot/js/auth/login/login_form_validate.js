import { apiHandler } from "../../utils/apiHandler"

document.addEventListener("DOMContentLoaded", function () {
  let form = document.getElementById("form")
  form.addEventListener("submit", function () {
    const data = new URLSearchParams(new FormData(form))
    apiHandler("post", "/api/auth/login", data)
  })
})



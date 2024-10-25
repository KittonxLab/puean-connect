document.addEventListener("DOMContentLoaded", function () {
  let profileImageWrapper = document.getElementById("profile-image-wrapper")
  let imageUpload = document.getElementById("image-upload")
  let profileImage = document.getElementById("profile-image")

  profileImageWrapper.addEventListener("click", function () {
    imageUpload.click()
  })

  imageUpload.addEventListener("change", function () {
    if (this.files && this.files[0]) {
      let reader = new FileReader()
      let validImageTypes = ["image/jpeg", "image/png", "image/gif"]
      if (validImageTypes.includes(this.files[0].type)) {
        reader.onload = function (e) {
          profileImage.src = e.target.result
        }
        reader.readAsDataURL(this.files[0])
      }
      else {
        alert("Please upload a valid image file.")
      }
    }
  })
})
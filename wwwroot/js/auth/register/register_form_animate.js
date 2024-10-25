function changeFormAnimate(curForm, nextForm, isSlidingRight) {
  function clearAnimation(e) {
    const allAnimation = ["sliding-out-right", "sliding-out-left", "sliding-in-right", "sliding-in-left"]
    for (const animation of allAnimation) {
      e.classList.remove(animation)
    }
  }

  clearAnimation(curForm)
  curForm.classList.add("sliding-out-" + (isSlidingRight ? "right" : "left"))

  curForm.addEventListener("animationend", function endAnimation() {
    curForm.style.display = "none"
    nextForm.style.display = "flex"

    clearAnimation(nextForm)
    nextForm.classList.add("sliding-in-" + (isSlidingRight ? "right" : "left"))

    curForm.removeEventListener("animationend", endAnimation)
  })
}

document.addEventListener("DOMContentLoaded", function () {
  let nextButton = document.getElementById("next-button")
  let prevButton = document.getElementById("prev-button")
  let credentialsForm = document.getElementById("credentials-form")
  let profileForm = document.getElementById("profile-form")
  let inputs = credentialsForm.querySelectorAll("input")

  nextButton.addEventListener("click", function () {
    for (const input of inputs) {
      if (!input.reportValidity())
        return
    }

    let password = document.getElementById("password").value
    let reTypePassword = document.getElementById("retype-password").value
    if (password != reTypePassword) {
      alert("Passwords do not match")
      return
    }

    changeFormAnimate(credentialsForm, profileForm, false)
  })

  prevButton.addEventListener("click", function () {
    changeFormAnimate(profileForm, credentialsForm, true)
  })
})

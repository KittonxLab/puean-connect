document.addEventListener("DOMContentLoaded", function () {
    const submitBtn = document.getElementById("disapprove-link");
    const editForm = document.getElementById("edit-form");
    
    const profilePWDValidation = () => {
        let password = document.getElementById("password").value;
        let reTypePassword = document.getElementById("retype-password").value;
        
        if (password != reTypePassword) {
            alert("Passwords do not match");
            this.location.href = "#user-creadential";
            return false
        }
        else {
            return true
        }
    }
    
    submitBtn.addEventListener("click", () => {
        if (profilePWDValidation()) {
            editForm.submit();
        }
    })
});
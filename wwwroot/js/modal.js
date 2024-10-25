const modal = document.getElementById("modal");
const openModals = document.querySelectorAll(".disapprove");
const closeModal = document.getElementById("modal-close-button");
const disApproveLink = document.getElementById("disapprove-link");

for (const openModal of openModals) {
  openModal.addEventListener("click", (e) => {
    if(e.currentTarget.getAttribute("link") !== null){
      disApproveLink.setAttribute("href", e.currentTarget.getAttribute("link"));
    }
    modal.showModal();
  });
}

closeModal.addEventListener("click", () => {
  modal.close();
});

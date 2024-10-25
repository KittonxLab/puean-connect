const shareBtn = document.querySelector(".share-btn");

shareBtn.addEventListener("click", async () => {
    await navigator.clipboard.writeText(location.href);
    alert("Copied Event URL to Clipboard");
});
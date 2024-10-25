const container = document.querySelector(".preview-img-container");
const uploadBtn = document.querySelector(".event-thumbnail-info input[type=file]");

window.addEventListener("load", () => {
    let uploadedImg = document.querySelector(".uploaded-img");
    let thumbnail = document.querySelector(".thumbnail-placeholder");
    console.log(uploadedImg, typeof thumbnail.value, thumbnail.value)
    if (uploadedImg == null && 
        (typeof thumbnail.value !== 'undefined' && thumbnail.value != "") ) {
            createPreviewImg(thumbnail.value);
    }
});

uploadBtn.addEventListener("change", () => {
    let [file] = uploadBtn.files;
    let uploadedImg = document.querySelector(".uploaded-img");
    
    if (!file) return 0;
    if (uploadedImg !== null) {
        uploadedImg.src = URL.createObjectURL(file);
        return 0;
    }
    
    createPreviewImg(URL.createObjectURL(file));
});

function createPreviewImg(fileUrl) {
    let previewImg = document.createElement("div");
    let image = document.createElement("img");
    let removeBtn = document.createElement("button");
    let icons = document.createElement("i");
    
    previewImg.className = "preview-img";
    image.className ="uploaded-img";
    
    removeBtn.type = "button";
    removeBtn.className = "remove-img-btn";
    removeBtn.onclick = removePreviewImg;
    
    icons.className = "fa-solid fa-circle-xmark fa-2xl";
    
    removeBtn.appendChild(icons)
    
    image.src = fileUrl;
    
    previewImg.appendChild(image);
    previewImg.appendChild(removeBtn);
    container.insertBefore(previewImg, container.firstChild )
}

function removePreviewImg() {
    uploadBtn.file = null;
    uploadBtn.value = null;
    let previewImg = document.querySelector(".preview-img");
    previewImg.remove();
    alert("Image Removed");
}


dateValidation();
numberValidation("min-rep", "min-rep-validation", 0, 100);
numberValidation("max-parti", "max-parti-validation", 1, 99);

function numberValidation(element, elementValidation, min, max) {
    const _element = document.getElementById(element);
    const submitBtn = document.getElementById("submit-btn");
    
    _element.addEventListener('change', () => {
        let spanValidation =  document.getElementById(elementValidation);
        if (_element && _element.value < min || _element.value > max) {
            spanValidation.innerHTML = `${min}-${max} only`;
            // spanValidation.style.marginLeft = "1rem";
            spanValidation.style.color = "red";
            spanValidation.style.fontSize = "0.8rem";
            location.href = "#"
            submitBtn.disabled = true;
        }
        else {
            spanValidation.innerHTML = "";
            submitBtn.disabled = false;
        }
        
    });
}

function dateValidation() {
    const eventDate = document.getElementById("event-date");
    const closeDate = document.getElementById("close-date");
    const submitBtn = document.getElementById("submit-btn");
    
    let compare = () => {
        var msgSpan = document.querySelector(".date-validation");
        let startValue = (new Date(eventDate.value)).getTime();
        let endValue = (new Date(closeDate.value)).getTime();
        
        let diff = (startValue - endValue)/(1000 * 60 * 60);
    
        if (Number.isNaN(diff) || diff < 48) {
            msgSpan.innerHTML = "Close Date should be at least 2 days before Event Date";
            msgSpan.style.marginLeft = "1rem";
            msgSpan.style.color = "red";
            msgSpan.style.fontSize = "0.8rem";
            submitBtn.disabled = true;
        } 
        else {
            msgSpan.innerHTML = '';
            submitBtn.disabled = false;
        }
        
        location.href = "#"
    };
    
    eventDate.addEventListener('change', compare);
    closeDate.addEventListener('change', compare);
}
document.addEventListener("DOMContentLoaded", function () {
  let resultPages = document.getElementById("results-container").children;
  let numberOfPage = resultPages.length;

  let pageNumbers = document.getElementById("page-numbers");
  let curPage = 1;

  let numberOfResults =
    (numberOfPage - 1) * 10 + resultPages[numberOfPage - 1].children.length;

  function renderResultNumber() {
    document.getElementById("result-number").textContent = `
      Showing ${
        resultPages[curPage - 1].children.length
      } groups out of ${numberOfResults} results found
    `;
  }

  renderResultNumber();

  function clearActive() {
    for (pageNumber of pageNumbers.children) {
      pageNumber.classList.remove("active");
    }
  }

  function changeActivePageNumber() {
    let i = Math.max(Math.min(curPage - 2, numberOfPage - 4), 1);
    let j = i;
    clearActive();
    for (pageNumber of pageNumbers.children) {
      pageNumber.textContent = i;
      if (i == curPage) {
        pageNumbers.children[i - j].classList.add("active");
      }
      ++i;
    }
    renderResultNumber();
  }

  function clearAnimation(e) {
    const allAnimation = ["sliding-out-left", "sliding-in-left"];
    for (const animation of allAnimation) {
      e.classList.remove(animation);
    }
  }

  function slideIn() {
    page = resultPages[curPage - 1];
    page.classList.add("sliding-in-left");
    page.style.display = "block";

    page.addEventListener(
      "animationend",
      function endAnimation() {
        clearAnimation(page);
      },
      { once: true }
    );
  }

  function slideOut(page) {
    page.classList.add("sliding-out-left");

    page.addEventListener(
      "animationend",
      function endAnimation() {
        page.style.display = "none";
        clearAnimation(page);
        slideIn();
      },
      { once: true }
    );
  }

  for (let i = 0; i < Math.min(numberOfPage - 1, 4); ++i) {
    let page = document.createElement("div");
    page.className = "page-number";
    page.innerHTML = `${i + 2}`;

    pageNumbers.append(page);
  }

  for (pageNumber of pageNumbers.children) {
    pageNumber.addEventListener("click", function () {
      let prevPage = curPage;
      curPage = parseInt(this.textContent);
      if (prevPage != curPage) {
        slideOut(resultPages[prevPage - 1]);
        changeActivePageNumber();
      }
    });
  }

  document.getElementById("arrow-left").addEventListener("click", function () {
    let prevPage = curPage;
    curPage = Math.max(1, curPage - 1);
    if (prevPage != curPage) {
      slideOut(resultPages[prevPage - 1]);
      changeActivePageNumber();
    }
  });

  document.getElementById("arrow-right").addEventListener("click", function () {
    let prevPage = curPage;
    curPage = Math.min(numberOfPage, curPage + 1);
    if (prevPage != curPage) {
      slideOut(resultPages[prevPage - 1]);
      changeActivePageNumber();
    }
  });
});
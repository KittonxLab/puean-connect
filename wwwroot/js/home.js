const categories = [
    "All",
    "Hang out",
    "Meeting",
    "Sports",
    "Travel",
    "Dining",
    "Tutoring",
    "Gaming",
];
let selectedCategory = "All";

const categoryAll = document.getElementById("All");
// categoryAll.style.backgroundColor = "var(--primary-light)";

const setSelectedCategory = (category) => {
    selectedCategory = category;
    categories.map((categoryItem) => {
        const tagElement = document.getElementById(categoryItem);
        tagElement.style.backgroundColor =
            selectedCategory === categoryItem ? "var(--primary-light)" : "";
    });

    filterEventsByCategory();
};

// Only one input for each for now
// const displayFilter = { category: [], search: [], sort: [] };

/* 
Each key contain function that decide each event should be visible or not
The function come from user's input, category filter and sort options
*/
// by default events is sorted from close to now to far from now in the controller
const displayFilter = {
    checkCategory: () => true,
    checkSearch: () => true,
    sortComparisonFunction: () => 0,
};

// onchange in input
const searchEvents = () => {
    const searchInput = document.getElementById("event-search-input");
    const searchText = searchInput.value.toLowerCase().trim();

    // every time filter logic happens, set currentPage back to one
    currentPage = 1;

    switch (searchText[0]) {
        case "#":
            displayFilter.checkSearch = (eventCard) => {
                const eventTags = eventCard.querySelector(".event-tags");
                const tagsArray = Array.from(
                    eventTags.querySelectorAll("p")
                ).map((p) => p.textContent.toLowerCase());

                return tagsArray.includes(searchText.slice(1).toLowerCase());
            };
            break;

        case "@":
            displayFilter.checkSearch = (eventCard) => {
                const eventId =
                    eventCard.querySelector(".event-id").textContent;

                return eventId.includes(searchText.slice(1));
            };
            break;

        default:
            displayFilter.checkSearch = (eventCard) => {
                const eventTitle = eventCard
                    .querySelector(".event-title")
                    .textContent.toLowerCase();

                return eventTitle.includes(searchText);
            };
    }

    displayEvents();
};

// onclick in category
const filterEventsByCategory = () => {
    // every time filter logic happens, set currentPage back to one
    currentPage = 1;

    if (selectedCategory === "All") {
        displayFilter.checkCategory = () => true;
        displayEvents();
        return;
    }

    displayFilter.checkCategory = (eventCard) => {
        const eventCategory =
            eventCard.querySelector(".event-category").textContent;
        return eventCategory === selectedCategory;
    };

    displayEvents();
};

// onchange in select, just set the way events are sorted
const setSortEvents = () => {
    const sortBy = document.getElementById("sort-dropdown").value;

    // return how events should be sorted
    switch (sortBy) {
        case "upcoming":
            displayFilter.sortComparisonFunction = (a, b) => {
                const eventDateA = new Date(
                    a.querySelector(".event-time").textContent.trim()
                );
                const eventDateB = new Date(
                    b.querySelector(".event-time").textContent.trim()
                );
                return eventDateA - eventDateB;
            };
            break;

        case "almost full":
            displayFilter.sortComparisonFunction = (a, b) => {
                const participantsA =
                    parseInt(
                        a.querySelector(".event-approved-participants")
                            .textContent
                    ) /
                    parseInt(
                        a.querySelector(".event-max-participants").textContent
                    );
                const participantsB =
                    parseInt(
                        b.querySelector(".event-approved-participants")
                            .textContent
                    ) /
                    parseInt(
                        b.querySelector(".event-max-participants").textContent
                    );
                // sort show near full first
                return participantsB - participantsA;
            };
            break;

        default:
            // Default comparison function (no sorting)
            displayFilter.sortComparisonFunction = () => 0;
    }

    // actual sorting logic is in paginate
    paginateEvents();
};

const displayEvents = () => {
    const eventCards = document.querySelectorAll(".event-card");
    // const eventGrids = document.querySelectorAll(".event-grid");

    // filtering
    eventCards.forEach((eventCard) => {
        const isSearched = displayFilter.checkSearch(eventCard);
        const isCorrectCategory = displayFilter.checkCategory(eventCard);

        eventCard.style.display =
            isSearched && isCorrectCategory ? "flex" : "none";
    });

    // actual sorting is in paginate
    paginateEvents();
    // setSortEvents();
};

const paginateEvents = () => {
    const eventCards = document.querySelectorAll(".event-card");
    const eventCardsArray = Array.from(eventCards);

    const eventGrids = document.querySelectorAll(".event-grid");
    const eventContent = document.getElementById("event-content");

    // remove old events to append new filterd one
    eventCards.forEach((eventCard) => {
        eventCard.remove();
    });
    eventGrids.forEach((eventGrid) => {
        eventGrid.remove();
    });

    // sort events before paginate
    eventCardsArray.sort(displayFilter.sortComparisonFunction);

    let i = 0;
    let firstBatch = true;

    while (i < eventCardsArray.length) {
        let visible = 0;
        const eventGridDiv = document.createElement("div");

        eventGridDiv.setAttribute("id", "event-grid");
        eventGridDiv.setAttribute("class", "event-grid");

        // every time filter/search/sort change it show the first page
        if (!firstBatch)
            eventGridDiv.setAttribute("class", "event-grid event-grid-hidden");

        while (visible < 10 && i < eventCardsArray.length) {
            eventGridDiv.appendChild(eventCardsArray[i]);

            if (eventCardsArray[i].style.display !== "none") {
                visible++;
            }
            i++;
        }

        firstBatch = false;
        eventContent.appendChild(eventGridDiv);
    }

    // TODO: bug when going from upcoming to almost full
    const pageNumbers = document.querySelectorAll(".event-page-number");
    console.log(pageNumbers);

    pageNumbers.forEach((pageNumber) => {
        pageNumber.remove();
    });
    addPageNumberDiv();

    // The number of result change (in changeActivePageNumber) every time paginate happen
    changeActivePageNumber();
};

let currentPage = 1;

const getNumberOfVisibleEvents = (eventCards) => {
    let numberOfVisibleEventCards = 0;

    eventCards.forEach((eventCard) => {
        if (getComputedStyle(eventCard).display === "flex") {
            numberOfVisibleEventCards++;
        }
    });

    return numberOfVisibleEventCards;
};

// Get number of visible events directly
const renderResultNumber = () => {
    const resultPages = document.getElementById("event-content").children;

    const allEventCards = document.querySelectorAll(".event-card");

    const numberOfResults = getNumberOfVisibleEvents(allEventCards);

    const numberOfEventsInCurrentPage = getNumberOfVisibleEvents(
        Array.from(resultPages[currentPage - 1].children)
    );

    document.getElementById("event-result-number").textContent = `
        Showing ${numberOfEventsInCurrentPage} of ${numberOfResults} results found`;
};

const clearActivePageNumber = () => {
    const pageNumbers = document.getElementById("event-page-numbers");

    for (pageNumber of pageNumbers.children) {
        pageNumber.classList.remove("active");
    }
};

const setShowingPageNumber = () => {
    clearActivePageNumber();

    console.log(`currentPage = ${currentPage}`);

    // This calculate the lowest number of the 5 number shown
    // example: press 4 on 1 2 3 4 5 will show 2 3 4 5 6 if there're enough pages for that
    const eventGrids = document.querySelectorAll(".event-grid");
    const numberOfPage = eventGrids.length;
    const minShownPageNumber = Math.min(
        Math.max(1, currentPage - 2),
        Math.max(1, numberOfPage - 4)
    );
    const pageNumberOffset = minShownPageNumber;

    const pageNumbers = document.getElementById("event-page-numbers");
    let i = minShownPageNumber;

    const pageNumberParagraphs = pageNumbers.querySelectorAll("p");

    for (pageNumber of pageNumberParagraphs) {
        pageNumber.textContent = i;
        if (i == currentPage) {
            pageNumbers.children[i - pageNumberOffset].classList.add("active");
        }
        ++i;
    }
};

const addPageNumberDiv = () => {
    const pageNumbers = document.getElementById("event-page-numbers");
    const eventGrids = document.querySelectorAll(".event-grid");
    const numberOfPage = eventGrids.length;

    for (let i = 1; i <= Math.min(numberOfPage, 5); ++i) {
        const page = document.createElement("div");
        page.className =
            i === 1 ? "event-page-number active" : "event-page-number";

        const pageNumber = document.createElement("p");
        // The number is replace in setShowingPageNumber anyways
        pageNumber.innerHTML = `${i}`;

        // How do I get pageNumber.innerHTML instead of page here?
        pageNumber.addEventListener("click", (event) => {
            currentPage = parseInt(event.target.innerHTML);
            changeActivePageNumber();
        });

        page.appendChild(pageNumber);

        pageNumbers.append(page);
    }
};

const changeActivePageNumber = () => {
    const eventGrids = document.querySelectorAll(".event-grid");

    let page = 1;
    eventGrids.forEach((eventGrid) => {
        if (currentPage === page) {
            eventGrid.classList.remove("event-grid-hidden");
        } else {
            eventGrid.classList.add("event-grid-hidden");
        }

        page++;
    });

    // document.getElementById("event-page-number").textContent = currentPage;

    setShowingPageNumber();

    const maxPages = document.querySelectorAll(".event-grid").length;
    const willBeTooHigh = currentPage + 1 > maxPages;
    const willBeTooLow = currentPage - 1 <= 0;

    document.getElementById("event-arrow-left").style.visibility = willBeTooLow
        ? "hidden"
        : "visible";

    document.getElementById("event-arrow-right").style.visibility =
        willBeTooHigh ? "hidden" : "visible";

    renderResultNumber();
};

const turnToNextPage = () => {
    const maxPages = document.querySelectorAll(".event-grid").length;
    const willBeTooHigh = currentPage + 1 > maxPages;

    currentPage = willBeTooHigh ? currentPage : currentPage + 1;
    changeActivePageNumber();
};

const turnToPreviousPage = () => {
    const willBeTooLow = currentPage - 1 <= 0;

    currentPage = willBeTooLow ? currentPage : currentPage - 1;
    changeActivePageNumber();
};

const navigateTo = (url) => {
    window.location.href = url;
};

document.addEventListener("DOMContentLoaded", function () {
    categoryAll.style.backgroundColor = "var(--primary-light)";
    document.getElementById("event-arrow-left").style.visibility = "hidden";

    // paginate it immediately
    paginateEvents();
});

const handleShare = async (event, eventId) => {
    event.stopPropagation();

    let path = location.href;
    const lastSlashIndex = path.lastIndexOf("/");
    if (lastSlashIndex !== -1) {
        path = path.substring(0, lastSlashIndex + 1); // Add 1 to include the last '/'
    }

    await navigator.clipboard.writeText(`${path}Event?id=${eventId}`);
    alert(`Copied Event URL to Clipboard`);
};

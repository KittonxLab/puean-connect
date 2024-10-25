// This is my array, that shouldn't be an issue
const routes = ["home", "your events", "group", "history"];
const routePaths = ["Home", "YourEvents", "Groups", "History"];
const icons = ["house", "book", "user-group", "clock-rotate-left"];
// let selectedRoute = localStorage.getItem("navRoute") ?? "home";

const navProfile = document.getElementById("nav-profile");
const navLogout = document.getElementById("nav-logout");

const setSelectedRoute = (currentPath) => {
  let selectedRoute;


  routePaths.map((routePath) => {
    if (routePath.toLowerCase() === currentPath.toLocaleLowerCase()) {
      selectedRoute = routes[routePaths.indexOf(routePath)]
    }
  });

    routes.map((routeItem) => {
        const routeElement = document.getElementById(routeItem);
        const routeInFullNavElement = document.getElementById(
            routeItem + "-full"
        );

        routeElement.style.backgroundColor =
            selectedRoute === routeItem ? "var(--primary-light)" : "";
        routeInFullNavElement.style.backgroundColor =
            selectedRoute === routeItem ? "var(--primary-light)" : "";
    });
    console.log(selectedRoute);
};

/*
know the route

for all icon (icon index = route index)
if icon in that route -> fa-solid
else -> fa-regular
*/

// some fa-regular for some icon need pros
const setSelectedIcon = (route) => {
    const routeIndex = routes.indexOf(route);
    icons.map((icon) => {
        if (icons.indexOf(icon) === routeIndex) {
            const iconElement = document.getElementById(`icon-${icon}`);
            iconElement.classList.remove("fa-solid");
            iconElement.classList.add("fa-regular");
        } else {
            const iconElement = document.getElementById(`icon-${icon}`);
            iconElement.classList.remove("fa-regular");
            iconElement.classList.add("fa-solid");
        }
    });
};

const handleRouteClick = (route) => {
  // localStorage.setItem("navRoute", route);
  const path = routePaths[routes.indexOf(route)];
  window.location.href = `${window.location.origin}/${path}`;
};

navProfile.addEventListener("mouseenter", () => {
    console.log("Mouse entered navProfile");
    navLogout.style.display = "flex";
});

navProfile.addEventListener("click", () => {
    console.log("Mouse entered navProfile");
    navLogout.style.display =
        navLogout.style.display === "none" ? "flex" : "none";
});

navProfile.addEventListener("mouseleave", () => {
    console.log("Mouse left navProfile");
    navLogout.style.display = "none";
});

navLogout.addEventListener("mouseenter", () => {
    console.log("Mouse entered navLogout");
    navLogout.style.display = "flex";
});

navLogout.addEventListener("mouseleave", () => {
    console.log("Mouse left navLogout");
    navLogout.style.display = "none";
});

const navigateToInNav = (url) => {
    console.log("this is clicked lol");
    window.location.href = url;
};

// mainContent.addEventListener('click', () => {
//   document.body.classList.toggle('nav-open');
// });

const toggleFullNav = () => {
    const mainContent = document.getElementById("main-content");
    const mainElement = document.getElementById("main");

    mainContent.classList.toggle("nav-open");
    mainElement.classList.toggle("main-scroll-hidden");
    console.log("full nav opened");
};

document.addEventListener("DOMContentLoaded", function () {
  console.log(`path name = ${window.location.pathname.split("/")[1]}`);
  let pathLocation = window.location.pathname.split("/")[1];
  const currentPath = pathLocation !== "" ? pathLocation : "Home";

  // Because JS is the best language, "/Home".split("/") is ["", "Home"], so I have to get [1]
  // routePaths.map((routePath) => {

  //   // console.log(`route = ${routePath.toLowerCase()}`)
  //   if (routePath.toLowerCase() === currentPath.toLocaleLowerCase()) {
  //     console.log("test route");
  //     // localStorage.setItem("navRoute", routes[routePaths.indexOf(routePath)]);
  //   }
  // });

  setSelectedRoute(currentPath);
});

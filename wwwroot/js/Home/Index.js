import { getCookies } from "../utils/getCookies.js";

let btn = document.querySelector("testBTN");
        addEventListener("click", async (e) => {
            document.cookie = "username = hi;";
            document.cookie = "username2=hi2;";
            let temp = getCookies();
            console.log(temp)
});
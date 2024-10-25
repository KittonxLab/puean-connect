
import { getCookies } from "./getCookies.js";

function setHeader(setAuthorization) {    
    const cookies = getCookies();
    let headers = { 
        "Accept": "application/json",
        "Content-Type": "application/json",
    };
    
    let accessToken = cookies["access"];
    if (setAuthorization) {
        if (accessToken === undefined) {
            window.location.replace(baseURL + "/login");
        }
        headers["Authorization"] = "Bearer " + accessToken;
    }
    
    return headers;
}

export async function apiHandler(
    method,
    path,
    data,
    setAuthorization=false,
    controller=new AbortController(),
    cache=true
) {
    try {
        const baseURL = "http://localhost:5010/"
        const response = await fetch(
            baseURL + path, 
            {
                method: method, // *GET, POST, PUT, PATCH, DELETE
                signal: controller.signal,
                cache: cache ? "default": "no-cache",
                credentials: "same-origin",
                headers: setHeader(setAuthorization),
                redirect: "follow",
                body: JSON.stringify(data),
            }
        );
        
        switch(response.status) {
            case 401:
                window.location.replace(baseURL + "login");
                break;
            case 404:
                window.location.replace(baseURL + "error/404");
                break;
            case 403:
                window.location.replace(baseURL);
                break;
            default:
                return await response.json();
        }
    }
    catch(error) {
        return { msg: error.message };
    }
}
export function getCookies() {
    let result = {};
    
    let cookies = document.cookie.split("; ");
    cookies.forEach((element) => {
        //split key and value
        let [ key, value ] = element.split("=", 2);
        
        result[key] = value;
    });
    
    return result;
}

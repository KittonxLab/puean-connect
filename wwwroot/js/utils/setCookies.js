export function setCookies(cookies) {
    try {
        
        let keys = Object.keys(cookies);
        keys.forEach(key => {
            document.cookies = `${key}=${cookies[key]}`;
        });
        
        return 0;
    }
    catch (error) {
        console.log(error.message);
    }
}
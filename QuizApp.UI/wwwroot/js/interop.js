export function ScrollTo(elementId, timeout) {
    setTimeout(() => {
        var element = document.getElementById(elementId);
        element.scrollIntoView({
            behavior: 'smooth'
        });
    }, timeout);
}
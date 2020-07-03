window.acblogInteropSetTitle = function (newTitle) {
    document.title = newTitle;
};
window.acblogInteropScrollTo = function (id) {
    var scrollToElement = document.getElementById(id);
    if (scrollToElement && scrollToElement.offsetTop) {
        window.scrollTo(0, scrollToElement.offsetTop);
    }
};
window.acblogInteropCopyItem = function (ele) {
    ele.select();
    document.execCommand('copy');
};
window.acblogInteropLoadingInfoShow = function () {
    var loadingInfo = document.getElementById("loading-info-ui");
    loadingInfo.style = "display: block";
};
window.acblogInteropLoadingInfoHide = function () {
    var loadingInfo = document.getElementById("loading-info-ui");
    loadingInfo.style = "display: none";
};
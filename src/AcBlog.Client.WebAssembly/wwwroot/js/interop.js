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
}
window.acblogInteropModalAction = function (id, action) {
    var m = $('#' + id);
    if (m) {
        m.modal(action);
    }
};
window.acblogInteropToastAction = function (id, action) {
    var m = $('#' + id);
    if (m) {
        m.modal(action);
    }
};
window.acblogInteropTooltipEnable = function () {
    $('[data-toggle="tooltip"]').tooltip();
};
window.acblogInteropPopoverEnable = function () {
    $('[data-toggle="popover"]').popover()
};
window.acblogInteropLoadingInfoShow = function () {
    var loadingInfo = document.getElementById("loading-info-ui");
    loadingInfo.style = "display: block";
}
window.acblogInteropLoadingInfoHide = function () {
    var loadingInfo = document.getElementById("loading-info-ui");
    loadingInfo.style = "display: none";
}

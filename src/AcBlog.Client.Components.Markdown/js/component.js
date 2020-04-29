"use strict";

var md = require("markdown-it")();

window.AcBlogClientComponentsMarkdown_markdownRender = function (element, content) {
    element.innerHTML = md.render(content);
}
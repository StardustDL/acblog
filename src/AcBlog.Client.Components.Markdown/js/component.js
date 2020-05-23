"use strict";

const hljs = require('highlight.js');
const katex = require('katex');

window.AcBlogClientComponentsMarkdown_highlight = function () {
    // hljs.initHighlighting();
    document.querySelectorAll('pre code').forEach((block) => {
        hljs.highlightBlock(block);
    });
}

window.AcBlogClientComponentsMarkdown_math = function () {
    var tex = document.getElementsByClassName("math");
    Array.prototype.forEach.call(tex, function (el) {
        var raw = el.textContent.trim();
        var displayMode = false;
        if (raw.startsWith("\\(") && raw.endsWith("\\)")) {
            raw = raw.substring(2, raw.length - 2).trim();
            displayMode = false;
        }
        else if (raw.startsWith("\\[") && raw.endsWith("\\]")) {
            raw = raw.substring(2, raw.length - 2).trim();
            displayMode = true;
        }
        katex.render(raw, el, {
            displayMode: displayMode
        });
    });
}
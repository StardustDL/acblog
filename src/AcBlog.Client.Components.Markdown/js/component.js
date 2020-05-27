"use strict";

const hljs = require('highlight.js');
const katex = require('katex');
var hasInitDiagram = false;

window.AcBlogClientComponentsMarkdown_highlight = function (element) {
    // hljs.initHighlighting();
    element.querySelectorAll('pre code').forEach((block) => {
        hljs.highlightBlock(block);
    });
}

window.AcBlogClientComponentsMarkdown_math = function (element) {
    var tex = element.getElementsByClassName("math");
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

const S4 = function() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = function() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

window.AcBlogClientComponentsMarkdown_diagram = function (element) {
    if (!hasInitDiagram) {
        mermaid.mermaidAPI.initialize({
            startOnLoad: false
        });
        hasInitDiagram = true;
    }

    var ls = element.getElementsByClassName("mermaid");
    Array.prototype.forEach.call(ls, function (c) {
        var raw = c.textContent;
        var id = "diagram" + guid();
        var insertSvg = function (svgCode, bindFunctions) {
            c.innerHTML = svgCode;
        };
        mermaid.mermaidAPI.render(id, raw, insertSvg, c);
    });
}

window.AcBlogClientComponentsMarkdown_fixAnchor = function (element, baseUrl) {
    var ls = element.getElementsByTagName("a");
    Array.prototype.forEach.call(ls, function (c) {
        var frag = decodeURI(c.href.replace(document.baseURI, ""));
        if (frag.startsWith("#")) {
            c.href = baseUrl + frag;
        }
    });
}
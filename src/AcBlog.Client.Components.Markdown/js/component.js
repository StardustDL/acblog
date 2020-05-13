"use strict";

const hljs = require('highlight.js');
const md = require("markdown-it")({
    html: true,
    linkify: true,
    typographer: true,
    highlight: function (str, lang) {
        if (lang && hljs.getLanguage(lang)) {
            try {
                return '<pre class="hljs"><code>' +
                    hljs.highlight(lang, str, true).value +
                    '</code></pre>';
            } catch (__) { }
        }
        return ''; // use external default escaping
    }
})
    .use(require("markdown-it-abbr"))
    .use(require("markdown-it-container"))
    .use(require("markdown-it-deflist"))
    .use(require("markdown-it-emoji"))
    .use(require("markdown-it-footnote"))
    .use(require("markdown-it-ins"))
    .use(require("markdown-it-sub"))
    .use(require("markdown-it-sup"))
    .use(require("markdown-it-mark"))
    .use(require("markdown-it-task-lists"))
    .use(require("@iktakahiro/markdown-it-katex"))
    .use(require('markdown-it-toc-and-anchor').default, {
        anchorLinkPrefix: "md-anchor-",
        anchorClassName: "markdownIt-Anchor",
        tocClassName: "markdownIt-TOC",
    });

window.AcBlogClientComponentsMarkdown_markdownRender = function (element, content, tocElementId, baseUrl) {
    element.innerHTML = md.render(content, {
        tocCallback: function (tocMarkdown, tocArray, tocHtml) {
            if (tocElementId) {
                document.getElementById(tocElementId).innerHTML = tocHtml;
            }
        },
    });
    var ls = document.getElementsByClassName("markdownIt-Anchor");
    for (let i = 0; i < ls.length; i++) {
        var c = ls[i];
        c.href = baseUrl + c.href.replace(document.baseURI, "");
    }
    var tocEle = document.getElementsByClassName("markdownIt-TOC")[0];
    if (tocEle) {
        var ls = tocEle.getElementsByTagName("a");
        for (let i = 0; i < ls.length; i++) {
            var c = ls[i];
            c.href = baseUrl + decodeURI(c.href.replace(document.baseURI, ""));
        }
    }
}
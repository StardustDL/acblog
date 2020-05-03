"use strict";

var abbr = require("markdown-it-abbr");
var container = require("markdown-it-container");
var deflist = require("markdown-it-deflist");
var emoji = require("markdown-it-emoji");
var footnote = require("markdown-it-footnote");
var ins = require("markdown-it-ins");
var sub = require("markdown-it-sub");
var sup = require("markdown-it-sup");
var mark = require("markdown-it-mark");
var tasklists = require("markdown-it-task-lists");
var katex = require("@iktakahiro/markdown-it-katex");
var hljs = require('highlight.js');
var md = require("markdown-it")({
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
    .use(abbr)
    .use(container)
    .use(deflist)
    .use(emoji)
    .use(footnote)
    .use(ins)
    .use(sub)
    .use(sup)
    .use(mark)
    .use(tasklists)
    .use(katex);

window.AcBlogClientComponentsMarkdown_markdownRender = function (element, content) {
    element.innerHTML = md.render(content);
}
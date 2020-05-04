"use strict";

const hljs = require('highlight.js');
const md = require("markdown-it")({
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
    .use(require("@iktakahiro/markdown-it-katex"));

window.AcBlogClientComponentsMarkdown_markdownRender = function (element, content) {
    element.innerHTML = md.render(content);
}
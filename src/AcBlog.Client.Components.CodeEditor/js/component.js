window.AcBlogClientComponentsCodeEditor_createCodeEditor = function (element, model) {
    element.innerHTML = "";
    require(['vs/editor/editor.main'], function () {
        var editor = monaco.editor.create(element, {
            value: model.invokeMethod("GetValue"),
            language: "markdown",
            wordWrap: "on",
            minimap: {
                enabled: false
            },
        });
        editor.layout();
    });
}
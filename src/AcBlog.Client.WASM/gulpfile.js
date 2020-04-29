'use strict';

var gulp = require('gulp');

gulp.task('default', function () {
    return gulp.src("./node_modules/monaco-editor/min/**/*").pipe(gulp.dest('wwwroot/lib/monaco-editor'));
});
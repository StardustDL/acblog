'use strict';

var gulp = require('gulp');

gulp.task('monaco-editor', function () {
    return gulp.src("./node_modules/monaco-editor/min/**/*").pipe(gulp.dest('wwwroot/lib/monaco-editor'));
});

gulp.task('jquery', function () {
    return gulp.src("./node_modules/jquery/dist/*.min.*").pipe(gulp.dest('wwwroot/lib/jquery'));
});

gulp.task('bootstrap', function () {
    return gulp.src("./node_modules/bootstrap/dist/**/*.min.*").pipe(gulp.dest('wwwroot/lib/bootstrap'));
});

gulp.task('mdi', function () {
    return gulp.src("./node_modules/@mdi/font/{css,fonts}/**/*").pipe(gulp.dest('wwwroot/lib/mdi'));
});

gulp.task('default', gulp.parallel('monaco-editor', 'bootstrap', 'mdi', 'jquery'));
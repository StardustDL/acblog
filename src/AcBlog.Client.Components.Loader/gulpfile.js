/// <binding />
'use strict';

const sass = require('gulp-sass')
const gulp = require('gulp');
const uglify = require('gulp-uglifycss');
const minify = require('gulp-clean-css');

gulp.task('component', function () {
    return gulp.src("css/component.scss")
        .pipe(sass())
        .pipe(uglify())
        .pipe(minify())
        .pipe(gulp.dest('wwwroot'));
});

gulp.task('default', gulp.parallel('component'));
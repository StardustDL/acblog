'use strict';

var browserify = require('browserify');
var gulp = require('gulp');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var log = require('gulplog');
var babel = require('gulp-babel');

gulp.task('markdown', function () {
    // set up the browserify instance on a task basis
    var b = browserify({
        entries: ['js/component.js'],
        debug: true
    });

    return b.bundle()
        .pipe(source('component.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        // Add transformation tasks to the pipeline here.
        .pipe(babel({
            presets: ['@babel/env']
        }))
        .pipe(uglify())
        .on('error', log.error)
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('wwwroot'));
});

gulp.task('highlight', function () {
    return gulp.src("./node_modules/highlight.js/styles/*").pipe(gulp.dest('wwwroot/highlight.js/'));
});

gulp.task('katex', function () {
    return gulp.src("./node_modules/katex/dist/**/*").pipe(gulp.dest('wwwroot/katex/'));
});

gulp.task('default', gulp.parallel('markdown', 'highlight', 'katex'));
/// <binding />
'use strict';

const browserify = require('browserify');
const gulp = require('gulp');
const source = require('vinyl-source-stream');
const buffer = require('vinyl-buffer');
const uglify = require('gulp-uglify');
const sourcemaps = require('gulp-sourcemaps');
const log = require('gulplog');
const babel = require('gulp-babel');
const minify = require('gulp-minify');

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
        .pipe(minify())
        .on('error', log.error)
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('wwwroot'));
});

gulp.task('highlight', function () {
    return gulp.src("./node_modules/highlight.js/styles/*").pipe(gulp.dest('wwwroot/highlight.js/'));
});

gulp.task('mermaid', function () {
    return gulp.src("./node_modules/mermaid/dist/*").pipe(gulp.dest('wwwroot/mermaid/'));
});

gulp.task('katex', function () {
    return gulp.src("./node_modules/katex/dist/**/*").pipe(gulp.dest('wwwroot/katex/'));
});

gulp.task('default', gulp.parallel('markdown', 'highlight', 'katex', 'mermaid'));
/// <binding />
'use strict'

var gulp = require('gulp')

gulp.task('jquery', function () {
  return gulp.src('./node_modules/jquery/dist/*.min.*').pipe(gulp.dest('wwwroot/lib/jquery'))
})

gulp.task('bootstrap', function () {
  return gulp.src('./node_modules/bootstrap/dist/**/*.min.*').pipe(gulp.dest('wwwroot/lib/bootstrap'))
})

gulp.task('markdown-css', function () {
  return gulp.src('./node_modules/github-markdown-css/*.css').pipe(gulp.dest('wwwroot/lib/markdown-css'))
})

gulp.task('default', gulp.parallel('bootstrap', 'jquery', 'markdown-css'))

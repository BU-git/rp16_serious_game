/// <binding />
"use strict";

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  webpack = require('webpack-stream'),
  uglify = require("gulp-uglify");


var paths = {
    webroot: "./wwwroot/"
};

paths.js = "./Assets/js/**/*.js";
paths.css = "./Assets/css/**/*.css";
paths.json = "./Assets/json/**/*.json";
paths.libs = "./Libraries/**/*";
paths.images = "./images/**/*";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "assets/js/site.min.js";
paths.concatCssDest = paths.webroot + "assets/css/site.min.css";
paths.libsDest = paths.webroot + "lib";
paths.imagesDest = paths.webroot + "assets/images";
paths.jsonDest = paths.webroot + "assets/json";
paths.assets = paths.webroot + "assets/";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:libs", function (cb) {
    rimraf(paths.libsDest, cb);
});

gulp.task("clean:assets", function(cb) {
    rimraf(paths.webroot, cb);
});

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], {
        base: "."
    })
      .pipe(concat(paths.concatJsDest))
      .pipe(uglify())
      .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
      .pipe(concat(paths.concatCssDest))
      .pipe(cssmin())
      .pipe(gulp.dest("."));
});

gulp.task("copy:libraries", function () {
    return gulp.src(paths.libs, { base: './Libraries' })
      .pipe(gulp.dest(paths.libsDest));
});

gulp.task("copy:images", function () {
    return gulp.src(paths.images, { base: './Assets' })
      .pipe(gulp.dest(paths.imagesDest));
});

gulp.task("copy:json", function () {
    return gulp.src(paths.json)
      .pipe(gulp.dest(paths.jsonDest));
});


gulp.task("clean", ["clean:js", "clean:css", "clean:assets"]);
gulp.task("min", ["min:js", "min:css"]);
gulp.task("copy", ["copy:libraries", "copy:images", "copy:json"]);
gulp.task("default", ["clean", "min", "copy"]);
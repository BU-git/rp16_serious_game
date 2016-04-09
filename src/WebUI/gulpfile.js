<<<<<<< HEAD
﻿/// <binding Clean='clean' />
=======
﻿/// <binding BeforeBuild='default' Clean='clean' />
>>>>>>> ad5dab3... Init login functionality + gulp tweaks
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
<<<<<<< HEAD

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
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

gulp.task("min", ["min:js", "min:css"]);
=======
paths.libsDest = paths.webroot + "lib";
paths.imagesDest = paths.webroot + "images";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:libs", function (cb) {
    rimraf(paths.libsDest, cb);
});

gulp.task("clean:all", function (cb) {
    rimraf(paths.webroot, cb);
})

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


gulp.task("clean", ["clean:js", "clean:css", "clean:libs", "clean:all"]);
gulp.task("min", ["min:js", "min:css"]);
gulp.task("copy", ["copy:libraries", "copy:images"]);
gulp.task("default", ["clean", "min", "copy"]);
>>>>>>> ad5dab3... Init login functionality + gulp tweaks

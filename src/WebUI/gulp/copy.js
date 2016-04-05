'use strict';

module.exports = (gulp, plugins, config) => () => {
  gulp.src(config.paths.images)
    .pipe(gulp.dest(`${config.paths.dist}/${config.output.images}`));

  gulp.src(config.paths.css)
    .pipe(gulp.dest(`${config.paths.dist}/${config.output.css}`));

  return gulp.src(config.paths.html)
    .pipe(gulp.dest(`${config.paths.dist}/${config.output.html}`));
};

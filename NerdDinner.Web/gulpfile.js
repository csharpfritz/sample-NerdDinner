/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  fs = require("fs"),
  less = require('gulp-less'),
  watch = require('gulp-watch'),
  batch = require('gulp-batch'),
  uglify = require('gulp-uglifyjs');

var project = {
  webroot: "./wwwroot"
};

var paths = {
  bower: "./bower_components/",
  lib: "./" + project.webroot + "/lib/",
  views: "./" + project.webroot + "/views/",
  styles: "./" + project.webroot + "/css/",
  images: "./" + project.webroot + "/images/",
};

gulp.task("clean", function (cb) {
  rimraf(paths.lib, cb);
});

gulp.task("copy", ["clean"], function () {
  var bower = {
    "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
    "bootstrap-touch-carousel": "bootstrap-touch-carousel/dist/**/*.{js,css}",
    "hammer.js": "hammer.js/hammer*.{js,map}",
    "jquery": "jquery/jquery*.{js,map}",
    "jquery-validation": "jquery-validation/jquery.validate.js",
    "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
    "angular": "angular/angular*.{js,map}",
    "angular-bootstrap": "angular-bootstrap/ui-bootstrap*.js",
    "angular-resource": "angular-resource/angular-resource*.{js,map}",
    "angular-route": "angular-route/angular-route*.js"
  }

  for (var destinationDir in bower) {
    gulp.src(paths.bower + bower[destinationDir])
      .pipe(gulp.dest(paths.lib + destinationDir));
  }

  gulp.src('ng-apps/views/*.html')
    .pipe(gulp.dest(paths.views));

  gulp.src('ng-apps/content/images/*.*')
    .pipe(gulp.dest(paths.images));
});

gulp.task('less', function () {
    return gulp.src('ng-apps/content/styles/*.less')
      .pipe(less())
      .pipe(gulp.dest(paths.styles));
});

gulp.task('uglify', function () {
    return gulp.src('ng-apps/**/*.js')
      .pipe(uglify('app.js', {
          mangle: false,
          output: {
              beautify: true
          }
      }))
      .pipe(gulp.dest(project.webroot));
});

gulp.task('buildNg', ['copy', 'uglify']);

gulp.task('watch', function() {

  watch("ng-apps/**", batch(function(events,done) {
    gulp.start('buildNg', done);
  }));

})

// TODO: gulp default task that makes ready for publish
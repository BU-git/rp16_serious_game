module.exports = {
    paths: {
        entry: './js/index.js',
        js: './Assets/js/**/*.js',
        html: './Assets/html/**/*.html',
        css: './Assets/css/**/*.css',
        dist: './wwwroot',
        images: './Assets/images/**/*'
    },
    output: {
        js: 'js',
        css: 'css',
        images: 'images',
        html: 'html'
    },
    production: false
};
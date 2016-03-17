/*global module:false*/
module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        // Task configuration.
        less: {
            production: {
                options: {
                    modifyVars: {
                        'font-url': '"../fonts/fonttiny"'
                    }
                },
                files: {
                    "dist/css/tiny.css": "src/less/tiny.less"
                }
            }
        },
        copy: {
            main: {
                files: [{
                    expand: true,
                    src: ['src/assets/bootstrap-theme-tiny/bootstrap.css'],
                    dest: 'dist/css/',
                    flatten: true
                }, {
                    expand: true,
                    src: ['src/assets/bootstrap-theme-tiny/bootstrap.js'],
                    dest: 'dist/js/', flatten: true
                }, {
                    expand: true,
                    src: ['src/fonts/*'],
                    dest: 'dist/fonts/', flatten: true
                }, {
                    expand: true,
                    src: ['src/js/*'],
                    dest: 'dist/js/',
                    filter: 'isFile', flatten: true
                }]
            },
            assets: {
                files: [{
                    expand: true,
                    cwd: 'bower_components/',
                    src: [
                        'font-awesome/**/*',
                        'jquery.scrollbar/**/*',
                        'html5shiv/**/*',
                        'respond/**/*',
                        'jquery/**/*',
                        'jquery.scrollbar/**/*',
                        'jquery.layout/**/*',
                        'jquery.fixedheadertable/**/*'
                    ],
                    dest: 'site/assets'
                }, {
                    expand: true,
                    cwd: 'dist/',
                    src: ['**/*'],
                    dest: 'site/assets/tiny'
                }, {
                    expand: true,
                    cwd: 'src/assets/',
                    src: ['demo/**/*'],
                    dest: 'site/assets/'
                }, {
                    src: 'src/assets/demo/index.html',
                    dest: 'site/index.html'
                }]
            }
        },
        clean: {
            include: ["src/tpls/_build"]
        },
        includes: {
            build: {
                cwd: 'src/tpls/_includes',
                src: ['**/*.*'],
                dest: 'src/tpls/_build'
            }
        },
        kss: {
            options: {
                "config": "src/kss-config.json"
            },
            dist: {
            }
        },
        jade: {
            compile: {
                files: [{
                    expand: true,
                    cwd: "src/tpls/_jade",
                    src: "*.jade",
                    dest: "site/examples",
                    ext: ".html"
                }]
            }
        }
    });

    // These plugins provide necessary tasks.
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-includes');
    grunt.loadNpmTasks('grunt-kss');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-jade');

    // Default task.
    grunt.registerTask('release', ['less', 'copy']);
    grunt.registerTask('site', ['release', 'includes', 'copy:assets', 'jade', 'kss', 'clean:include']);
    grunt.registerTask('default', ['release']);

};

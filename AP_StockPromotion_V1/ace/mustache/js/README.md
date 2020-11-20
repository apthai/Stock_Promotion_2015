This folder contains the js files to create raw html output.

Use like this:
  node index.js

It uses Hogan.js by default.
To use Mustache.js:
  node index.js

Extra options are like 'app/data/common/site.json'
For example:
  node index.js --remote_jquery=true


node index.js --onpage_help=true --development=true
node ajax.js --onpage_help=true --development=true
node index.js --output_folder="demo" --path_minified="\.min" --path_base="." --path_assets="dist" --path_images="assets/images" --demo=true --onpage_help=false --development=false --protocol=false --remote_jquery=true --remote_fonts=true --remote_bootstrap_js=true --remote_fontawesome=true
node ajax.js --output_folder="demo" --path_minified="\.min" --path_base=".../" --path_assets=".../dist" --path_images=".../assets/images" --demo=true --onpage_help=false --development=false --protocol=false --remote_jquery=true --remote_fonts=true --remote_bootstrap_js=true --remote_fontawesome=true
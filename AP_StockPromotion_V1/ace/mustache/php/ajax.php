<?php
//This php file acts as a very basic simple controller, loading data and view and generating output
$page_name = isset($_GET['page']) ? trim($_GET['page']) : NULL;


include 'vendor/Mustache/Autoloader.php';
Mustache_Autoloader::register();
spl_autoload_register(function ($class) {
	if(!is_file('classes/' . $class . '.php')) return;
	include 'classes/' . $class . '.php';
});


$path = array(
 'data' => __DIR__ . '/../app/data',
 'views' => __DIR__ . '/../app/views',
 'assets' => '../../assets',
 'base' => '../..',
 'images' => '../../assets/images',
 'minified' => ''
);
$site = json_decode(file_get_contents($path['data'].'/common/site.json'));//this site some basic site variables
$site->protocol = '';//no protocol, so the page's default (http or https) will be used
if($site->development == true) {
 $site->ace_scripts = array();
 $scripts = json_decode(file_get_contents($path['assets'].'/js/ace/scripts.json'));
 $ajax_script = 'ace.ajax-content.js';
 $scripts->$ajax_script = true;
 
 foreach($scripts as $name => $include) {
	if($include) $site->ace_scripts[] = $name;
 }
}
$site->ajax = true;


//if no such page, then show 404 page!
if($page_name && !is_file($path['data']."/pages/{$page_name}.json")) $page_name = "error-404";

$page = NULL;
$sidenav = new Sidenav();

if($page_name != null) {
	$page = new Page( array('path' => $path, 'name' => $page_name, 'type' => 'page') );
	$layout_name = 'ajax-content';//$page->get_var('layout');
}
else $layout_name = 'ajax-layout';


$layout = new Page( array('path' => $path, 'name' => $layout_name, 'type' => 'layout') );


if($navList = &$layout->get_var('sidebar_items'))
{
	$sidenav->set_items($navList);
	$sidenav->mark_active_item($page_name);
}



//now make an engine, with custom loader, pass page&layout name to it and let it autoload!
$engine = new Mustache_Engine(array(
	'cache' => '_cache',
	'partials_loader' => new CustomLoader($path['views'] , array('layout' => $layout_name, 'page' => $page ? $page->get_name() : ''))
));



$context = array( "page" => $page ? $page->get_vars() : array() , "layout" => $layout->get_vars(), "path" => $path , "site" => $site);
$context['breadcrumbs'] = $sidenav->get_breadcrumbs();

$context['createAjaxLinkFunction'] = function($value) {
 return '#page/'.$value;
};
$context['createLinkFunction'] = function($value) {
 return 'index.php?page='.$value;
};


echo $engine->render($layout->get_template(), $context);
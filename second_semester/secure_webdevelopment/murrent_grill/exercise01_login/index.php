<?php
$uv = '2jf75eop#a20ß3kdhg733!';

session_start();
if(isset($_POST['login']))
{
	$password = '$2y$10$AX0g6kDwyvur7qDOzg9AfufsggY6tRwK4ZSYIUD36be24PBcWw46S';
	
	if(isset($_POST['password']) && isset($_POST['username'])){
				?>
		</br>
		<?php
		if(password_verify($_POST['password'], $password)){
			$_POST['uid'] = "001A1";
			
			$_SESSION['username'] = $_POST['username'];
			
			echo "Success";
		}else{
			echo "Failed";
		}
	}
}
?>
<!DOCTYPE html>
<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <title>Login Form</title>
</head>
<body>
  <section class="container">
    <div class="login">
      <h1>Login to Web App</h1>
      <form method="POST">
        <p><input type="text" name="username" value="" placeholder="Username"></p>
        <p><input type="password" name="password" value="" placeholder="Password"></p>
        <p><input type="submit" name="login" value="Login"></p>
      </form>
	</div>
  </section>
</body>
</html>

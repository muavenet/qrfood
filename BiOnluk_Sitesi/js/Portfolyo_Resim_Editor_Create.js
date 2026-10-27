
var $resup = jQuery.noConflict();
var sayi = 0;
var dizi = new Array();
var silinecek = new Array();

var newlabel = document.createElement("Label");
newlabel.id = "lblDosyaAdet";

window.onload = $resup.ajax(
	{


		type: "POST",
		url: "/Portfolyo/FotolariGetir",
		//data: "{'isIlanId' : "1f9f4740-0faa-4b38-85bd-85a208741fb"}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",

		success: function (markers) {


			//Videoları Getir



			//console.log(markers);

			//markers = markers.d;

			//console.log(markers);


			var ana = document.getElementById("pr_content");
			var angleInDegrees = 0;

			if (markers.length == 0)
				document.getElementById("tooltip").style.display = "none";

			for (var i = 0; i < markers.length; i++) {

				yukle(markers[i].FotoPath, markers[i].ilanName);

			}

			function yukle(path, file_name) {

				var canvas_ = document.createElement("canvas");
				var ctx_ = canvas_.getContext("2d");

				canvas_.id = file_name;
				canvas_.title = "";

				var btn = document.createElement("button");
				btn.id = "pre_btn";
				btn.type = "button";
				btn.setAttribute("title", "Resmi çevir");
				btn.setAttribute("alt", "Resmi çevir");

				var btnVitrin = document.createElement("button");
				//btn.innerHTML = "Çevir";
				btnVitrin.id = "pre_btn_vitrin";
				btnVitrin.type = "button";
				btnVitrin.setAttribute("title", "Kapak resmi yap");
				btnVitrin.setAttribute("alt", "Kapak resmi yap");

				var btnDelete = document.createElement("button");
				//btn.innerHTML = "Çevir";
				btnDelete.id = "pre_btn_delete";
				btnDelete.type = "button";
				btnDelete.setAttribute("title", "Resmi sil");
				btnDelete.setAttribute("alt", "Resmi sil");


				var preview_inside = document.createElement("div");
				preview_inside.id = "pvi";

				var preview_inside_dest = document.createElement("div");
				preview_inside_dest.id = "pvid";

				ana.appendChild(preview_inside);
				var img = new Image();
				img.src = '../../' + path;
				//console.log(img.src);

				img.onload = function () {

					ctx_.save();

					canvas_.style.width = 190 + "px";

					canvas_.style.height = 150 + "px";

					canvas_.width = img.width;

					canvas_.height = img.height;

					ctx_.drawImage(img, 0, 0, img.width, img.height);

					ctx_.restore();

					preview_inside.appendChild(preview_inside_dest);

					preview_inside.appendChild(canvas_);

					preview_inside_dest.appendChild(btn);

					preview_inside_dest.appendChild(btnVitrin);

					preview_inside_dest.appendChild(btnDelete);

					$resup(btn).on("click", function () {

						document.getElementById("rot_image").className = "";

						FileName = file_name;

						ctx_.restore();

						document.getElementById("rot_image").src = canvas_.toDataURL();

						document.getElementById('cerceve_arka').style.visibility = 'visible';

					});

					$resup(btnDelete).on("click", function () {

						//console.log("Resim silme");

						silinecek.push(canvas_.id);

						//console.log("canvas id =" + canvas_.id);


						document.getElementById("pr_content").removeChild(preview_inside);
						document.getElementById("lblDosyaAdet").innerHTML = document.getElementById("lblDosyaAdet").value - 1;
						document.getElementById("lblDosyaAdet").value = document.getElementById("lblDosyaAdet").value - 1;
						document.getElementById("progress_file").value = document.getElementById("progress_file").value - 1;

						if (document.getElementById("progress_file").value == 0)
							document.getElementById("tooltip").style.display = "none";

					});


					$resup(btnVitrin).on("click", function () {

						var div = document.getElementById('pr_content');
						var divs = div.getElementsByTagName('canvas');

						for (var i = 0; i < divs.length; i += 1) {
							if (divs[i].title == "Vitrin")
								divs[i].title = "";

						}

						$resup("div#pr_content").find("div#pvi").removeClass("black_box");


						$resup(this).closest("div#pvi").addClass("black_box");



						canvas_.title = "Vitrin";



						//divs.style.border = "solid 2px red";

						//document.getElementById("tooltip").style.border = "solid 2px red";


						//var rect = preview_inside.getBoundingClientRect();

						//var body = document.body;
						//var docElem = document.documentElement;

						//var clientTop = docElem.clientTop || body.clientTop || 0;
						//var clientLeft = docElem.clientLeft || body.clientLeft || 0;

						//var scrollTop = window.pageYOffset || docElem.scrollTop || body.scrollTop;
						//var scrollLeft = window.pageXOffset || docElem.scrollLeft || body.scrollLeft;

						//document.getElementById("tooltip").style.marginTop = "0";
						//$resup("#tooltip").hide().fadeIn("slow", "linear");

						//document.getElementById("tooltip").style.left = rect.left + scrollLeft - clientLeft - 9 + "px";
						//document.getElementById("tooltip").style.top = rect.top + scrollTop - clientTop + "px";


					});
					// İlk render
					$resup("#btnDondur").on("click", function () {

						if (canvas_.id == FileName) {
							if (angleInDegrees >= 360)
								angleInDegrees = 0;
							angleInDegrees += 90;
						}

						document.getElementById("rot_image").className = "img_transs_" + angleInDegrees + "";

					});

					$resup("#btnGom").on("click", function () {

						if (canvas_.id == FileName) {

							drawRotatedImage(angleInDegrees, document.getElementById("rot_image").src);

							document.getElementById('cerceve_arka').style.visibility = "hidden";

							angleInDegrees = 0;

							FileName = "";
						}

					});

					$resup("#btnVazgec").on("click", function () {


						document.getElementById('cerceve_arka').style.visibility = "hidden";

						angleInDegrees = 0;

						FileName = "";

					});

				}

				function drawRotatedImage(angle, image_rot) {

					ctx_.clearRect(0, 0, canvas_.width, canvas_.height);

					var Image_ = new Image();

					ctx_.save();

					Image_.src = image_rot;

					ctx_.translate(canvas_.width / 2, canvas_.height / 2);

					ctx_.rotate(angle * Math.PI / 180);

					ctx_.drawImage(Image_, -(Image_.width / 2), -(Image_.height / 2));

					ctx_.restore();

				};
			};


			var newlabel = document.createElement("Label");
			newlabel.id = "lblDosyaAdet";
			newlabel.value = markers.length;
			newlabel.innerHTML = markers.length;

			document.getElementById("dosya_adet").appendChild(newlabel);



			document.getElementById("progress_file").value = markers.length;
		},
		error: function () {
			//var image = document.createElement("input");
			//image.type = "image";
			//image.width = "110";
			//image.heigh = "90";

			//image.name = markers[i].ilanName;
			//image.src = markers[i].FotoPath;
			alert('Foto upload çalışmadı');
		}
	});

// Check for the various File API support.
if (window.File && window.FileReader && window.FileList && window.Blob) {

	var render = function (src, file_name) {


		var canvas_ = document.createElement("canvas");
		var ctx_ = canvas_.getContext("2d");


		var MAX_HEIGHT = 513;
		var MAX_Width = 700;
		var thumb_w;
		var thumb_h;
		var thumb_w_max = 110;
		var thumb_h_max = 100;
		var angleInDegrees = 0;

		var btn = document.createElement("button");
		//btn.innerHTML = "Çevir";
		//btn.id = "pre_btn";
		//btn.type = "button";
		//btn.setAttribute("title", "Resmi çevir");
		//btn.setAttribute("alt", "Resmi çevir");

		var btnVitrin = document.createElement("button");
		//btn.innerHTML = "Çevir";
		btnVitrin.id = "pre_btn_vitrin";
		btnVitrin.type = "button";
		btnVitrin.setAttribute("title", "Kapak resmi yap");
		btnVitrin.setAttribute("alt", "Kapak resmi yap");

		var btnDelete = document.createElement("button");
		//btn.innerHTML = "Çevir";
		btnDelete.id = "pre_btn_delete";
		btnDelete.type = "button";
		btnDelete.setAttribute("title", "Resmi sil");
		btnDelete.setAttribute("alt", "Resmi sil");


		var preview_inside = document.createElement("div");
		preview_inside.id = "pvi";
		preview_inside.draggable = "true";

		document.getElementById("pr_content").appendChild(preview_inside);

		var preview_inside_dest = document.createElement("div");
		preview_inside_dest.id = "pvid";

		var img = new Image();

		img.onload = function () {

			if (img.height > MAX_HEIGHT) {
				img.width *= MAX_HEIGHT / img.height;
				img.height = MAX_HEIGHT;
			}
			if (img.width > MAX_Width) {
				img.height *= MAX_Width / img.width;
				img.width = MAX_Width;
			}

			if (img.height > img.width) {
				thumb_w = img.width + 20;
				thumb_h = img.height;

				thumb_w *= thumb_h_max / thumb_h;
				thumb_h = thumb_h_max;
			}

			if (img.height < img.width) {

				thumb_w = img.width;
				thumb_h = img.height;

				thumb_h *= thumb_w_max / thumb_w;
				thumb_w = thumb_w_max;

			}

			//ctx_.clearRect(0, 0, canvas_.width, canvas_.height);
			ctx_.save();
			canvas_.style.width = 190 + "px";
			canvas_.style.height = 150 + "px";
			canvas_.width = img.width;
			canvas_.height = img.height;
			canvas_.id = file_name;
			canvas_.title = "";
			ctx_.drawImage(img, 0, 0, img.width, img.height);
			ctx_.restore();


			preview_inside.appendChild(preview_inside_dest);

			preview_inside.appendChild(canvas_);

			preview_inside_dest.appendChild(btn);
			preview_inside_dest.appendChild(btnVitrin);
			preview_inside_dest.appendChild(btnDelete);

			$resup(btn).on("click", function () {

				document.getElementById("rot_image").className = "";

				FileName = file_name;

				ctx_.restore();



				document.getElementById("rot_image").src = canvas_.toDataURL();

				document.getElementById('cerceve_arka').style.visibility = 'visible';

			});
			$resup(btnDelete).on("click", function () {

				silinecek.push(canvas_.id);
				document.getElementById("pr_content").removeChild(preview_inside);

				document.getElementById("lblDosyaAdet").innerHTML = document.getElementById("lblDosyaAdet").value - 1;
				document.getElementById("lblDosyaAdet").value = document.getElementById("lblDosyaAdet").value - 1;
				document.getElementById("progress_file").value = document.getElementById("progress_file").value - 1;

				if (document.getElementById("progress_file").value == 0)
					document.getElementById("tooltip").style.display = "none";

			});

			$resup(btnVitrin).on("click", function () {


				var div = document.getElementById('pr_content');
				var divs = div.getElementsByTagName('canvas');
				for (var i = 0; i < divs.length; i += 1) {
					if (divs[i].title == "Vitrin")
						divs[i].title = "";
				}

				$resup("div#pr_content").find("div#pvi").removeClass("black_box");

				$resup(this).closest("div#pvi").addClass("black_box");

				canvas_.title = "Vitrin";

				//var rect = preview_inside.getBoundingClientRect();

				//var body = document.body;
				//var docElem = document.documentElement;

				//var clientTop = docElem.clientTop || body.clientTop || 0;
				//var clientLeft = docElem.clientLeft || body.clientLeft || 0;

				//var scrollTop = window.pageYOffset || docElem.scrollTop || body.scrollTop;
				//var scrollLeft = window.pageXOffset || docElem.scrollLeft || body.scrollLeft;

				////document.getElementById("tooltip").style.marginTop = "0";

				//$resup("#tooltip").hide().fadeIn("slow", "linear");

				//document.getelementbyıd("tooltip").style.left = rect.left + scrollleft - clientleft - 9 + "px";
				//document.getelementbyıd("tooltip").style.top = rect.top + scrolltop - clienttop + "px";

				//console.log(angleInDegrees);

			});
			$resup("#btnDondur").on("click", function () {

				if (canvas_.id == FileName) {

					//console.log(angleInDegrees);

					if (angleInDegrees >= 360) angleInDegrees = 0;

					angleInDegrees += 90;

					document.getElementById("rot_image").className = "img_transs_" + angleInDegrees + "";

				}


			});
			$resup("#btnGom").on("click", function () {

				if (canvas_.id == FileName) {

					drawRotatedImage(angleInDegrees, document.getElementById("rot_image").src);

					document.getElementById('cerceve_arka').style.visibility = "hidden";

					angleInDegrees = 0;

					FileName = "";


				}

			});
			$resup("#btnVazgec").on("click", function () {


				document.getElementById('cerceve_arka').style.visibility = "hidden";

				angleInDegrees = 0;

				FileName = "";

			});

		};
		img.src = src;

		function drawRotatedImage(angle, image_rot) {

			ctx_.clearRect(0, 0, canvas_.width, canvas_.height);

			var Image_ = new Image();

			ctx_.save();

			Image_.src = image_rot;

			ctx_.translate(canvas_.width / 2, canvas_.height / 2);

			ctx_.rotate(angle * Math.PI / 180);

			ctx_.drawImage(Image_, -(Image_.width / 2), -(Image_.height / 2));

			ctx_.restore();

		};


	};

	var progress = document.querySelector('.percent');

	function updateProgress(evt) {
		// evt is an ProgressEvent.
		if (evt.lengthComputable) {
			var percentLoaded = Math.round((evt.loaded / evt.total) * 100);
			// Increase the progress bar length.
			if (percentLoaded < 100) {
				progress.style.width = percentLoaded + '%';
				progress.textContent = percentLoaded + '% yüklendi...';
			}

			if (percentLoaded == 100) {

			}
		}
	}

	var artis = 0;

	var readImage = function (imgFile) {

		progress.style.width = '0%';
		progress.textContent = '0%';



		var reader = new FileReader();

		if (!imgFile.type.match(/image.*/)) {
			//console.log("Dosya bir resim değil: ", imgFile.type);
			return;
		}

		reader.onprogress = updateProgress;

		reader.onloadstart = function (e) {
			document.getElementById('progress_bar').className = 'loading';
		};

		reader.onload = function (e) {

			render(e.target.result, imgFile.name);
			progress.style.width = 100 + '%';
			progress.textContent = 100 + '%';
			setTimeout("document.getElementById('progress_bar').className='';", 2500);
		};

		reader.readAsDataURL(imgFile);



	};

	function handleFileSelectBtn(evt) {

		var div = document.getElementById('pr_content');
		var divs = div.getElementsByTagName('canvas');
		var canvasAdet = divs.length;


		evt.stopPropagation();
		evt.preventDefault();


		var files = evt.target.files;



		for (var i = 0, f; f = files[i]; i++) {

			if (canvasAdet++ < 16) {
				//console.log(canvasAdet);
				readImage(f);

				if (document.getElementById("lblDosyaAdet").value != "16") {
					document.getElementById("lblDosyaAdet").innerHTML = document.getElementById("lblDosyaAdet").value + 1;
					document.getElementById("lblDosyaAdet").value = document.getElementById("lblDosyaAdet").value + 1;

					document.getElementById("progress_file").value = document.getElementById("progress_file").value + 1;
				}
			}

		}
		document.getElementById("tooltip").style.display = "block";


	};

}

else {
	alert('File APIs tarayıcı tarafından desteklenmiyor.');
}

function servisGonder(params) {

	var div = document.getElementById('pr_content');
	var divs = div.getElementsByTagName('canvas');
	var divArray = new Array();
	var ss;
	for (var i = 0; i < divs.length; i += 1) {

		if (divs[i].title == "Vitrin") {
			divArray.push("Vitrin-" + divs[i].toDataURL('image/jpeg', 90 / 100).substring(divs[i].toDataURL('image/jpeg', 90 / 100).indexOf(",") + 1) + "," + divs[i].id);
		}
		else {
			divArray.push(divs[i].toDataURL('image/jpeg', 90 / 100).substring(divs[i].toDataURL('image/jpeg', 90 / 100).indexOf(",") + 1) + "," + divs[i].id);
		}
	}

	var dataToPass = { ss: divArray, silinecek: silinecek };
	//console.log("dataToPass" + dataToPass);
	var jsonYazi = JSON.stringify(dataToPass);
	//console.log("jsonYazi" + jsonYazi);
	DosyalariKaydet(jsonYazi, params);


};

var btnID = '<%=btDevam.ClientID %>';

function DosyalariKaydet(jsonYazi, btnDevam, event) {

	$resup.ajax({

		url: "../../ResimYakala.asmx/YakalaDuzelt",
		data: jsonYazi,
		dataType: "json",
		type: "POST",
		async: false,
		contentType: "application/json; charset=utf-8",

		success: function (response) {

			$resup.post("/Portfolyo/ResimDuzenle_Metodu");

			$odeme('#rootwizard').bootstrapWizard('show', 3); // Next Tab


			var delay = 3000;
			var url = "/Portfolyo/Portfolyom";
			var timeoutID = setTimeout(function () {
				window.location.href = url;
			}, delay);



			window.onbeforeunload = () => { };
			//if (response.d == true) {
			//	__doPostBack(btnID, btnDevam);
			//	console.log("success");
			//}

		},
		error: function () {

			console.log("hata");

		}
	});
}

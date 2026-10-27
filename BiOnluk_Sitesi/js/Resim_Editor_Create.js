


	var $resup = jQuery.noConflict();

	var sayi = 0;
	var dizi = new Array();
	var newlabel = document.createElement("Label");
	newlabel.id = "lblDosyaAdet";

	// Check for the various File API support.
	if (window.File && window.FileReader && window.FileList && window.Blob) {

		var render = function (src, size, file_name) {

			var canvas = document.createElement("canvas");
			var ctx = canvas.getContext("2d");

			var MAX_HEIGHT = 394;
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

				//ctx.clearRect(0, 0, canvas.width, canvas.height);
				ctx.save();
				canvas.style.width = 110 + "px";
				canvas.style.height = 85 + "px";
				canvas.width = img.width;
				canvas.height = img.height;
				canvas.id = file_name;
				canvas.title = "Name";
				ctx.drawImage(img, 0, 0, img.width, img.height);
				ctx.restore();

				preview_inside.appendChild(preview_inside_dest);
				preview_inside.appendChild(canvas);

				preview_inside_dest.appendChild(btn);
				preview_inside_dest.appendChild(btnVitrin);
				preview_inside_dest.appendChild(btnDelete);

				$resup(btn).on("click", function () {

					document.getElementById("rot_image").className = "";

					FileName = file_name;

					ctx.restore();



					document.getElementById("rot_image").src = canvas.toDataURL();

					document.getElementById('cerceve_arka').style.visibility = 'visible';

				});
				$resup(btnDelete).on("click", function () {

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

					canvas.title = "Vitrin";

					var rect = preview_inside.getBoundingClientRect();

					var body = document.body;
					var docElem = document.documentElement;

					var clientTop = docElem.clientTop || body.clientTop || 0;
					var clientLeft = docElem.clientLeft || body.clientLeft || 0;

					var scrollTop = window.pageYOffset || docElem.scrollTop || body.scrollTop;
					var scrollLeft = window.pageXOffset || docElem.scrollLeft || body.scrollLeft;

					document.getElementById("tooltip").style.marginTop = "0";
					$resup("#tooltip").hide().fadeIn("slow", "linear");

					document.getElementById("tooltip").style.left = rect.left + scrollLeft - clientLeft - 9 + "px";
					document.getElementById("tooltip").style.top = rect.top + scrollTop - clientTop + "px";



				});


				$resup("#btnDondur").on("click", function () {

					if (canvas.id == FileName || canvas.id == "Vitrin") {

						if (angleInDegrees >= 360) angleInDegrees = 0;

						angleInDegrees += 90;

						document.getElementById("rot_image").className = "img_transs_" + angleInDegrees + "";

					}


				});



				$resup("#btnGom").on("click", function () {

					if (canvas.id == FileName) {

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

				ctx.clearRect(0, 0, canvas.width, canvas.height);

				var Image_ = new Image();

				ctx.save();

				Image_.src = image_rot;

				ctx.translate(canvas.width / 2, canvas.height / 2);

				ctx.rotate(angle * Math.PI / 180);

				ctx.drawImage(Image_, -(Image_.width / 2), -(Image_.height / 2));

				ctx.restore();

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
					document.getElementById("tooltip").style.visibility = "visible";

				}
			}


		}


		var artis = 0;
		var readImage = function (imgFile) {

			progress.style.width = '0%';
			progress.textContent = '0%';

			var reader = new FileReader();

			if (!imgFile.type.match(/image.*/)) {
				console.log("Dosya bir resim değil: ", imgFile.type);
				return;
			}

			reader.onprogress = updateProgress;

			reader.onloadstart = function (e) {
				document.getElementById('progress_bar').className = 'loading';
			};

			reader.onload = function (e) {

				render(e.target.result, imgFile.size, imgFile.name);
				progress.style.width = 100 + '%';
				progress.textContent = 100 + '%';
				setTimeout("document.getElementById('progress_bar').className='';", 2500);
			};


			reader.readAsDataURL(imgFile);



		};


		var dragSrcEl = null;



		var div = document.getElementById('pr_content');
		var divs = div.getElementsByTagName('canvas');

		function handleDrop(evt) {

			evt.stopPropagation();
			evt.preventDefault();



			var canvasAdet = divs.length;

			

			var files = evt.dataTransfer.files;


			for (var i = 0, f; f = files[i]; i++) {

				if (canvasAdet++ < 8) {

					readImage(f);

					if (document.getElementById("lblDosyaAdet").value < "8") {
						document.getElementById("lblDosyaAdet").innerHTML = document.getElementById("lblDosyaAdet").value + 1;
						document.getElementById("lblDosyaAdet").value = document.getElementById("lblDosyaAdet").value + 1;

						document.getElementById("progress_file").value = document.getElementById("progress_file").value + 1;
					}
				}

			}


			document.getElementById("tooltip").style.display = "block";
		};



		function handleFileSelectBtn(evt) {


			var canvasAdet = divs.length;

			console.log("girdi");

			evt.stopPropagation();
			evt.preventDefault();


			var files = evt.target.files;

		

			for (var i = 0, f; f = files[i]; i++) {

				if (canvasAdet++ < 8) {

					readImage(f);

					if (document.getElementById("lblDosyaAdet").value < "8") {
						document.getElementById("lblDosyaAdet").innerHTML = document.getElementById("lblDosyaAdet").value + 1;
						document.getElementById("lblDosyaAdet").value = document.getElementById("lblDosyaAdet").value + 1;

						document.getElementById("progress_file").value = document.getElementById("progress_file").value + 1;
					}
				}

			}



			document.getElementById("tooltip").style.display = "block";

		};

		function handleDragOver(evt) {
			evt.stopPropagation();
			evt.preventDefault();
			evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
		}

		// Setup the dnd listeners.
		var dropZone = document.getElementById('preview');
		dropZone.addEventListener('drop', handleDrop, false);
		dropZone.addEventListener('dragover', handleDragOver, false);


	}

	else {
		alert('File APIs tarayıcı tarafından desteklenmiyor.');
	}

	function required() {
		var flag = window.Page_ClientValidate('valid');

		return flag;
	}

	function servisGonder(params) {

	document.getElementById("load_ilanver").style.visibility = "visible";

	//console.log(document.getElementById("load_ilanver"));
	//if (required() == true)
	//{
	//console.log(required);

	ilkFonksiyon(params);
};

	function ilkFonksiyon(params) {

		var div = document.getElementById('pr_content');
		var divs = div.getElementsByTagName('canvas');
		var divArray = new Array();
		var ss;

		for (var i = 0; i < divs.length; i += 1) {

			if (divs[i].title == "Vitrin") {
				divArray.push("Vitrin-" + divs[i].toDataURL('image/jpeg', 90 / 100).substring(divs[i].toDataURL('image/jpeg', 90 / 100).indexOf(",") + 1));
			}
			else {
				divArray.push(divs[i].toDataURL('image/jpeg', 90 / 100).substring(divs[i].toDataURL('image/jpeg', 90 / 100).indexOf(",") + 1));
			}

		}
		var dataToPass = { ss: divArray };
		var jsonYazi = JSON.stringify(dataToPass);
		DosyalariKaydet(jsonYazi, params);
	};

	function DosyalariKaydet(jsonYazi, btnParam) {

		$resup.ajax({
			url: "../ResimYakala.asmx/Yakala",
			data: jsonYazi,
			dataType: "json",
			type: "POST",
			async: false,
			contentType: "application/json; charset=utf-8",

			success: function (response) {
				$resup.post("/MusteriPaneli/ResimEkle_Metodu");

				$odeme('#rootwizard').bootstrapWizard('show', 4); // Next Tab
				//TetikleDevam(btnParam);

				var delay = 3000;
				var url = "/MusteriPaneli/IsIlanlari";
				var timeoutID = setTimeout(function () {
					window.location.href = url;
				}, delay);

				window.onbeforeunload = () => { };
			},
			error: function (response) {
				console.log("hata :" + response);
			},
			complete: function () {
				document.getElementById("load_ilanver").style.visibility = "hidden";
			}
		});
	}

	function TetikleDevam(params) {
		var btnID = '<%=btDevam.ClientID %>';
		__doPostBack(btnID, params);
	}

	function dene(data) {


		var yaz = document.getElementById("<%= hdListe.ClientID %>");

		$resup.each(data, function (index, item) {

			yaz.value = yaz.value.toString() + "," + item.toString();

		});

		yaz.value = yaz.value.substring(yaz.value.indexOf(",") + 1);

		console.log(yaz.value);

	}

	window.onload = function () {

		newlabel.value = 0;
		newlabel.innerHTML = 0;

		document.getElementById("dosya_adet").appendChild(newlabel);
	}


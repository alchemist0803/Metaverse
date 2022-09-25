mergeInto(LibraryManager.library, {

  drg_init: function () {
    window.dragon.init();
  },

  drg_home: function() {
    window.dragon.init();
    window.dragon.goHomePage();
  },

  drg_save: function(background, gendor, clothes, hair, shoes, face, skin) {
    window.dragon.setCustomizeData(background, gendor, clothes, hair, shoes, face, skin);
  },

  drg_saveMap: function(data) {
    window.dragon.saveFavorite(data);
  },

  drg_saveCharacterImage: function(array, size) {
    let data = []
    for(var i = 0; i < size; i++) {
      data[i] = HEAPU8[array + i]
    }
    window.dragon.saveImage(data);
  },

  drg_saveImage: function(data, idx, gendor, hair, face, clothes, shoes, skin) {
    var strContent = UTF8ToString(data);
    if(idx == 0) {
      window.dragon.saveImage0(strContent, gendor, hair, face, clothes, shoes, skin);
    } else if(idx == 1) {
      window.dragon.saveImage1(strContent, gendor, hair, face, clothes, shoes, skin);
    } else if(idx == 2) {
      window.dragon.saveImage2(strContent, gendor, hair, face, clothes, shoes, skin);
    } else if(idx == 3) {
      window.dragon.saveImage3(strContent, gendor, hair, face, clothes, shoes, skin);
    } else if(idx == 4) {
      window.dragon.saveImage4(strContent, gendor, hair, face, clothes, shoes, skin);
    }
  },

  drg_showBtn: function(data) {
    window.dragon.showMoreBtn(data);
  },

  drg_getUserData: function(id) {
    window.dragon.getUserName(id);
  },

});
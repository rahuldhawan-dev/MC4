function sleep(seconds, fn) {
  window.setTimeout(fn, seconds * 1000);
}

// Town
pick("Town: listbox", "ABERDEEN");
// Street Number
enter(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[3]/TD[2]/INPUT[1]"), "123")
// Asset Type
pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[5]/TD[2]/SELECT[1]"), "Valve");
// Requested By
pick("Requested By: listbox", "Customer")
// Purpose
pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[8]/TD[2]/SELECT[1]"), "Customer");
// Priority
pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[8]/TD[4]/SELECT[1]"), "Emergency");
// Markout Requirement
pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[10]/TD[4]/SELECT[1]"), "Emergency");

sleep(1, function() {
  // Street
  pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[3]/TD[4]/SELECT[1]"), "ALDEN CT");
  // Nearest Cross Street
  pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[4]/TD[2]/SELECT[1]"), "1ST ST");
  // Customer Name
  enter(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[6]/TD[4]/INPUT[1]"), "John Doe");
  // Phone Number
  enter(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[7]/TD[2]/INPUT[1]"), "8885551212");
  // Description of Work
  pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[9]/TD[2]/SELECT[1]"), "INSTALL BLOW OFF VALVE");
  // Markout Number
  enter(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[10]/TD[4]/DIV[1]/INPUT[1]"), "123456789");

 sleep(1, function() {
    // Valve ID
    pick(new XPath("/HTML[1]/BODY[1]/FORM[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]/TABLE[1]/TBODY[1]/TR[3]/TD[1]/DIV[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/TABLE[1]/TBODY[1]/TR[5]/TD[4]/SELECT[2]"), "303")
  });
});







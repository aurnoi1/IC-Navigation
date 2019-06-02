@xunit:collection(UITests)
Feature: ViewBlue
	I want to validate that controls and features are correctly implemented

Background: 
    Given The application under test has been started
    And The "blue view" has been opened

@view_menu
Scenario: The controls should be displayed in "blue view"
    Then The following controls should be displayed in the current view:
    | usage_name                             |
    | title                                  |
    | button to go back to the previous view |
    | button to open the yellow view         |
